using CanInterface.Extensions;
using CanInterface.MCP2515.BitStructures;
using CanInterface.MCP2515.Enum;
using CanInterface.MCP2515.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515
{
    public partial class Mcp2515Controller : IMcp2515Controller
    {

        public TimeSpan ResetDefultTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public TimeSpan TransmitDefaultTimeout { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan ReceiveDefaultTimeout { get; set; } = TimeSpan.FromSeconds(1);


        protected Spi.ISpiDevice SpiDevice = null;


        public Mcp2515Controller(Spi.ISpiDevice device)
        {
            SpiDevice = device ?? throw new ArgumentNullException(nameof(device));
        }

        /// <summary>
        /// Resets the device
        /// </summary>
        /// <param name="resetTimeout"></param>
        /// <returns></returns>
        public void Init(BaudRate baudRate, byte frequency, SyncronizationJumpWidth syncJumpWidth)
        {
            Reset();

            //Reset the interrupts and transmit control registers
            WriteRegister(Registers.CANINTE, new CanInterruptEnableRegister(0));
            WriteRegister(Registers.TXB0CTRL, new TransmitBufferControlRegister((byte)0));
            WriteRegister(Registers.TXB1CTRL, new TransmitBufferControlRegister((byte)0));
            WriteRegister(Registers.TXB2CTRL, new TransmitBufferControlRegister((byte)0));

            if(!SetBaudRate(baudRate, frequency, (byte)((byte)syncJumpWidth >> 5)))
            {
                throw new InvalidConfigurationException($"Unable to correctly configure the device with the settings. BaudRate: {baudRate}. Freq: {frequency}. JumpWidth: {syncJumpWidth}");
            }
        }
        
        /// <summary>
        /// Resets the device
        /// </summary>
        /// <param name="resetTimeout"></param>
        /// <returns></returns>
        public void Reset(TimeSpan? resetTimeout = null)
        {
            SpiDevice.Write(Commands.RESET);

            var timeOut = DateTime.Now.Add(resetTimeout ?? ResetDefultTimeout);


            CanStatusRegister? status = null;
            while (DateTime.Now < timeOut && status?.OperatingMode != OperatingMode.Configuration)
            {
                status = ReadCanStatus();
            }

            if (status?.OperatingMode != OperatingMode.Configuration)
            {
                throw new TimeoutException($"Controller status did not change to Configuration withing the expected window. Status {status?.OperatingMode}");
            }
        }

        /// <summary>
        /// Gets the status values from the device
        /// </summary>
        /// <returns></returns>
        public CanStatusRegister ReadCanStatus()
        {
            return ReadRegister(Registers.CANSTAT);
        }

        /// <summary>
        /// Reads the interrupt flags from the device
        /// </summary>
        /// <returns></returns>
        public CanInterruptFlagRegister ReadInteruptFlags()
        {
            return ReadRegister(Registers.CANINTF);
        }

        /// <summary>
        /// Sends the given message on the first available buffer starting at buffer 0
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="timeout">The timeout </param>
        /// <returns>True if the message was sent, false if the timeout occured before the message was sent</returns>
        public bool Transmit(CanMessage message, TimeSpan? timeout = null)
        {
            byte ctrl = 0;
            byte intf = 0;

            var status = ReadStatus();
            LoadTxBuffer txBuffer;

            switch (status)
            {
                case ReadStatusInstructionResponse s when (!status.TXB0CNTRL_TXREQ && !status.CANINTF_TX0IF):
                    ctrl = Registers.TXB0CTRL;
                    intf = Registers.TX0IF;
                    txBuffer = Enum.LoadTxBuffer.TX0SIDH;
                    break;
                case ReadStatusInstructionResponse s when (!status.TXB1CNTRL_TXREQ && !status.CANINTF_TX1IF):
                    ctrl = Registers.TXB1CTRL;
                    intf = Registers.TX1IF;
                    txBuffer = Enum.LoadTxBuffer.TX1SIDH;
                    break;
                case ReadStatusInstructionResponse s when (!status.TXB2CNTRL_TXREQ && !status.CANINTF_TX2IF):
                    ctrl = Registers.TXB2CTRL;
                    intf = Registers.TX2IF;
                    txBuffer = Enum.LoadTxBuffer.TX2SIDH;
                    break;
                default:
                    throw new NoAvailableBuffersException(BufferType.Transmit);
            }

            //load the buffer with the data from the can message
            LoadTxBuffer(txBuffer, message);

            //tell the controller we are ready to transmit
            WriteRegisterBit(ctrl, Registers.TXREQ, 1);

            var timeoutTime = DateTime.Now.Add(timeout ?? TransmitDefaultTimeout);

            var messageSent = false;
            while(DateTime.Now < timeoutTime && !messageSent)
            {
                CanInterruptFlagRegister register = ReadRegister(Registers.CANINTF);

                switch (intf)
                {
                    case Registers.TX0IF:
                        messageSent = register.TX0IF;
                        break;
                    case Registers.TX1IF:
                        messageSent = register.TX1IF;
                        break;
                    case Registers.TX2IF:
                        messageSent = register.TX2IF;
                        break;
                }

            }

            //will abort the send if it has not happend by this point
            WriteRegisterBit(ctrl, Registers.TXREQ, 0);

            //clears the interrupt
            WriteRegisterBit(Registers.CANINTF, intf, 0);

            return messageSent;
        }


        /// <summary>
        /// Recieves a message from the given buffer
        /// </summary>
        /// <param name="buffer">The buffer to recieve the message from</param>
        /// <param name="timeout">A timeout to expire the read</param>
        /// <returns>
        ///     True when read, false if a timeout occured
        ///     The constructed message if one was read, null if not
        /// </returns>
        public (bool read, CanMessage message) Receive(ReceiveBuffer buffer = ReceiveBuffer.Both, TimeSpan? timeout = null)
        {
            var timeoutTime = DateTime.Now.Add(timeout ?? ReceiveDefaultTimeout);

            bool isRemote = false;
            byte[] rxBuffer = null; 

            while(DateTime.Now < timeoutTime && rxBuffer == null)
            {
                CanInterruptFlagRegister interrupts = ReadRegister(Registers.CANINTF);
                
                if((buffer == ReceiveBuffer.RX0 || buffer == ReceiveBuffer.Both) && interrupts.RX0IF)
                {
                    isRemote = ((ReceiveBuffer0ControlRegister)ReadRegister(Registers.RXB0CTRL)).RXRTR;
                    rxBuffer = ReadRxBuffer(Enum.ReadRxBuffer.RXB0SIDH);
                }
                else if((buffer == ReceiveBuffer.RX1 || buffer == ReceiveBuffer.Both) && interrupts.RX1IF)
                {
                    isRemote = ((ReceiveBuffer1ControlRegister)ReadRegister(Registers.RXB1CTRL)).RXRTR;
                    rxBuffer = ReadRxBuffer(Enum.ReadRxBuffer.RXB1SIDH);
                }
            }

            if(rxBuffer == null)
            {
                return (false, null);
            }
                       
            

            RxStandardIdentifierHighRegister sidhRegister = rxBuffer[0];
            RxStandardIdentifierLowRegister sidlRegister = rxBuffer[1];

            RxExtendendedIdentifier8Register? eid8Register = null;
            RxExtendendedIdentifier0Register? eid0Register = null;

            if (sidlRegister.IDE)
            {
                eid8Register = rxBuffer[2];
                eid0Register = rxBuffer[3];
            }

            RxDataLengthCodeRegister dlcRegister = rxBuffer[4];

            var data = new byte[dlcRegister.DLC];

            Array.ConstrainedCopy(rxBuffer, 5, data, 0, dlcRegister.DLC);

            return (true, new CanMessage(sidhRegister, sidlRegister, eid8Register, eid0Register, dlcRegister, data));
        }

        private bool SetBaudRate(BaudRate baudRate, byte frequency, byte syncJumpWidth)
        {
            if(baudRate != BaudRate.Auto)
            {
                SetBaudRate((int)baudRate, frequency, syncJumpWidth);
                return true;
            }

            for (int i = 5; i < 1000; i += 5)
            {
                try
                {
                    SetBaudRate(i, frequency, syncJumpWidth);
                    SetOperatingMode(OperatingMode.ListenOnly, ReceiveBufferOperatingMode.AcceptAll, ReceiveBufferOperatingMode.AcceptAll, true);
                    CanInterruptFlagRegister interrupt;

                    int attempts = 0;

                    do
                    {
                        Task.Delay(TimeSpan.FromMilliseconds(500)).Wait();
                        interrupt = ReadRegister(Registers.CANINTF);
                    }
                    while (!interrupt.RX0IF && !interrupt.RX1IF && ++attempts < 40);
                    
                    if (!interrupt.MERRF)
                    {
                        return true;
                    }

                    Reset();
                }
                catch(InvalidConfigurationException icex)
                {
                    //just eat the exception
                }


            }

            return false;

        }

        /// <summary>
        /// Sets the baud rate
        /// </summary>
        /// <param name="baudRate"></param>
        private void SetBaudRate(int baudRate, byte frequency, byte syncJumpWidth)
        {

            if(syncJumpWidth < 0)
            {
                syncJumpWidth = 0;
            }

            if(syncJumpWidth > 4)
            {
                syncJumpWidth = 4;
            }

            
            float timeQuantum = 0f;
            byte baudRatePrescaler = 0;
            byte bitTiming = 0;
            float tempBitTiming = 0f;


            float nominalBitTime = 1.0f / ((float)baudRate) * 1000.0f;

            for (baudRatePrescaler = 0; baudRatePrescaler < 8; baudRatePrescaler++)
            {
                timeQuantum = 2.0f * ((float)(baudRatePrescaler + 1)) / ((float)frequency);
                tempBitTiming = nominalBitTime / timeQuantum;
                if(tempBitTiming <= 25)
                {
                    bitTiming = (byte)tempBitTiming;
                    if(tempBitTiming - bitTiming == 0)
                    {
                        break;
                    }   
                }
            }

            byte samplePoint = (byte)(0.7 * bitTiming);
            byte propagationSegment = (byte)((samplePoint - 1) / 2);
            byte phaseSegment1 = (byte)(samplePoint - propagationSegment - 1);
            byte phaseSegment2 = (byte)(bitTiming - phaseSegment1 - propagationSegment - 1);

            if(propagationSegment + phaseSegment1 < phaseSegment2)
            {
                throw new InvalidConfigurationException($"PropagationSegment ({propagationSegment}), PhaseSegment1({phaseSegment1}) and PhaseSegment2({phaseSegment2}) are out of the expected ranges");
            }

            if(phaseSegment2 <= syncJumpWidth)
            {
                throw new InvalidConfigurationException($"PhaseSegment2({phaseSegment2}) is  out of the expected range");
            }


            var cnf1 = new Configuration1Register((SyncronizationJumpWidth)(syncJumpWidth << 5), baudRatePrescaler);
            WriteRegister(Registers.CNF1, cnf1);
            WriteRegister(Registers.CNF2, new Configuration2Register(true, false, (byte)(phaseSegment1 -1), (byte)(propagationSegment -1)));
            WriteRegister(Registers.CNF3, new Configuration3Register(false,false, (byte)(phaseSegment2 -1)));
            WriteRegister(Registers.TXRTSCTRL, new TransmitPinControlAndStatusRegister(false, false, false));

            var readCnf1 = (Configuration1Register)ReadRegister(Registers.CNF1);
            if(readCnf1 != cnf1)
            {
                throw new InvalidConfigurationException("the read configuration did not match the expected configuration at CNF1");
            }


        }

        /// <summary>
        /// Reads the value at the given register address
        /// </summary>
        /// <param name="register">The register address</param>
        /// <returns>The read value</returns>
        public byte ReadRegister(byte register)
        {
            var readBuffer = new byte[1];
            SpiDevice.TransferSequential(new[] { Commands.READ, register }, readBuffer);
            return readBuffer[0];
        }

        /// <summary>
        /// writes the values at the starting register
        /// </summary>
        /// <param name="register">The register to start writing at</param>
        /// <param name="value">the bytes to write</param>
        public void WriteRegister(byte register, byte[] value)
        {
            var toWrite = new byte[value.Length + 2];
            toWrite[0] = Commands.WRITE;
            toWrite[1] = register;
            Array.ConstrainedCopy(value, 0, toWrite, 2, value.Length);
            SpiDevice.Write(toWrite);
        }
        
        /// <summary>
        /// Writes the given value to the specified register
        /// </summary>
        /// <param name="register">The register to write to</param>
        /// <param name="value">The value to write</param>
        public void WriteRegister(byte register, byte value)
        {
            SpiDevice.Write(Commands.WRITE, register, value);
        }

        /// <summary>
        /// Sets the given bit of the given register  to the value specified
        /// </summary>
        /// <param name="register">The register to modify</param>
        /// <param name="bitNumber">The bit to modify</param>
        /// <param name="value">The value to set the bit to</param>
        public void WriteRegisterBit(byte register, byte bitNumber, byte value)
        {
            SpiDevice.Write(Commands.BIT_MODIFY, register, ((byte)(1 << bitNumber)), (byte)(value == 0 ? 0b0000_0000 : 0b1111_1111));
        }

        /// <summary>
        /// sends the request to send command
        /// </summary>
        /// <param name="transmit2">Send the tx0 buffer</param>
        /// <param name="transmit1">Send the tx1 buffer</param>
        /// <param name="transmit0">Send the tx2 buffer</param>
        public void RequestToSend(bool transmit2, bool transmit1, bool transmit0)
        {
            SpiDevice.Write(Commands.RTS.Set(2, transmit2).Set(1, transmit1).Set(0, transmit0));
        }

        /// <summary>
        /// Gets the status instruction response
        /// </summary>
        /// <returns>The status response</returns>
        public ReadStatusInstructionResponse ReadStatus()
        {
            var buffer = new byte[1];
            SpiDevice.TransferSequential(new byte[] { Commands.READ_STATUS }, buffer);

            return buffer[0];
        }

        /// <summary>
        /// Read the full or data only buffer
        /// </summary>
        /// <param name="location">The location to read from. </param>
        /// <returns>The data read from the registers. Will read the the registers in sequence:
        /// SIDH, SIDL, EID8, EID0, DLC, D0-D7</returns>
        public byte[] ReadRxBuffer(ReadRxBuffer location)
        {
            int bufferSize = 13;
            if(location == Enum.ReadRxBuffer.RXB0DO || location == Enum.ReadRxBuffer.RXB1D0)
            {
                bufferSize = 8;
            }

            byte[] buffer = new byte[bufferSize];

            SpiDevice.TransferSequential(new byte[] { (byte)location }, buffer);

            return buffer;
        }

        /// <summary>
        /// Loads the transmit buffer starting at the address indicated by teh the bufferToLoad 
        /// </summary>
        /// <param name="bufferToLoad">The transmit buffer to write to</param>
        /// <param name="values">the values to write. each byte will be written to the registers in sequence: 
        /// SIDH, SIDL, EID8, EID0, DLC, D0-D7</param>
        public void LoadTxBuffer(LoadTxBuffer bufferToLoad, byte[] values)
        {
            byte[] buffer = new byte[values.Length + 1];

            buffer[0] = (byte)bufferToLoad;
            Array.ConstrainedCopy(values, 0, buffer, 1, values.Length);

            SpiDevice.Write(buffer);
        }
                
        /// <summary>
        /// Sets the operating mode of the Device
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="rx0BufferMode"></param>
        /// <param name="rx1BufferMode"></param>
        /// <param name="rollOverBuffer0To1"></param>
        public void SetOperatingMode(OperatingMode mode, ReceiveBufferOperatingMode rx0BufferMode, ReceiveBufferOperatingMode rx1BufferMode, bool rollOverBuffer0To1)
        {
            var current = new CanControlRegister(ReadRegister(Registers.CANCTRL));

            var newOperatingMode = new CanControlRegister(mode, current.ABAT, current.OSM, current.CLKEN, current.CLKPRE);

            WriteRegister(Registers.CANCTRL, newOperatingMode);

            CanStatusRegister status = ReadRegister(Registers.CANSTAT);

            if(status.OperatingMode != newOperatingMode.REQOP)
            {
                throw new InvalidOperatingModeExcpetion(newOperatingMode.REQOP, status.OperatingMode);
            }

            //configure the read bufffers
            WriteRegister(Registers.RXB0CTRL, new ReceiveBuffer0ControlRegister(rx0BufferMode, rollOverBuffer0To1));
            WriteRegister(Registers.RXB1CTRL, new ReceiveBuffer1ControlRegister(rx1BufferMode));
            //enable all the registers
            WriteRegister(Registers.CANINTE, new CanInterruptEnableRegister(0b1111_1111));
        }


    }
}

