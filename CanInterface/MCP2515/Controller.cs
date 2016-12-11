using CanInterface.Extensions;
using CanInterface.MCP2515.BitStructures;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.Devices.Spi;

namespace CanInterface.MCP2515
{
    public class Controller
    {

        public TimeSpan ResetDefultTimeout { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan TransmitDefaultTimeout { get; set; } = TimeSpan.FromSeconds(1);
        public TimeSpan ReceiveDefaultTimeout { get; set; } = TimeSpan.FromSeconds(1);


        protected SpiDevice spiDevice = null;


        public Controller(SpiController contoller, int csPin)
        {
            spiDevice = contoller.GetDevice(new SpiConnectionSettings(csPin)
            {
                Mode = SpiMode.Mode0,
                DataBitLength = 8,
                SharingMode = SpiSharingMode.Exclusive
            });
        }


        public async Task Init(BaudRate baudRate)
        {
            await Reset();
            await SetBaudRateAsync(baudRate);
        }


        /// <summary>
        /// Resets the device
        /// </summary>
        /// <param name="resetTimeout"></param>
        /// <returns></returns>
        public Task Reset(TimeSpan? resetTimeout = null)
        {
            return Task.Run(async () => {
                spiDevice.Write(Commands.RESET);

                var timeOut = DateTime.Now.Add(resetTimeout ?? ResetDefultTimeout);


                CanStatusRegister? status = null;
                while (DateTime.Now < timeOut && status?.OperatingMode != OperatingMode.Configuration)
                {
                    status = ReadCanStatus();
                    await Task.Delay(100);
                }

                if(status?.OperatingMode != OperatingMode.Configuration)
                {
                    throw new TimeoutException($"Controller status did not change to Configuration withing the expected window. Status {status?.OperatingMode}");
                }

            });
        }

        /// <summary>
        /// Resets the device
        /// </summary>
        /// <returns></returns>
        public Task<CanStatusRegister> ReadCanStatusAsync()
        {
            return Task.Run(() => ReadCanStatus());
        }

        /// <summary>
        /// Gets the status values from the device
        /// </summary>
        /// <returns></returns>
        public CanStatusRegister ReadCanStatus()
        {
            return new CanStatusRegister(ReadRegister(Registers.CANSTAT));
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
            LoadTxBuffer(txBuffer, message.ToTransmitRegisterBytes());

            //tell the controller we are ready to transmit
            WriteRegisterBit(ctrl, Registers.TXREQ, 1);

            var timeoutTime = DateTime.Now.Add(timeout ?? TransmitDefaultTimeout);

            var messageSent = false;
            while(DateTime.Now < timeoutTime && !messageSent)
            {
                var register = new CanInterruptFlagRegister(ReadRegister(Registers.CANINTF));

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
            WriteRegisterBit(intf, intf, 0);

            return messageSent;
        }

        /// <summary>
        /// Sends the given message on the first available buffer starting at buffer 0
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="timeout">The timeout </param>
        /// <returns>True if the message was sent, false if the timeout occured before the message was sent</returns>
        public Task<bool> TransmitAsync(CanMessage message, TimeSpan? timeout = null)
        {
            return Task.Run(() => Transmit(message, timeout));
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
        public (bool read, CanMessage message) Receive(ReceieveBuffer buffer = ReceieveBuffer.Both, TimeSpan? timeout = null)
        {
            var timeoutTime = DateTime.Now.Add(timeout ?? ReceiveDefaultTimeout);

            bool isRemote = false;
            byte[] rxBuffer = null; 

            while(DateTime.Now < timeoutTime && rxBuffer == null)
            {
                var interrupts = new CanInterruptFlagRegister(ReadRegister(Registers.CANINTF));
                
                if((buffer == ReceieveBuffer.RX0 || buffer == ReceieveBuffer.Both) && interrupts.RX0IF)
                {
                    isRemote = new ReceiveBuffer0Control(ReadRegister(Registers.RXB0CTRL)).RXRTR;
                    rxBuffer = ReadRxBuffer(Enum.ReadRxBuffer.RXB0SIDH);
                }
                else if((buffer == ReceieveBuffer.RX1 || buffer == ReceieveBuffer.Both) && interrupts.RX1IF)
                {
                    isRemote = new ReceiveBuffer1Control(ReadRegister(Registers.RXB1CTRL)).RXRTR;
                    rxBuffer = ReadRxBuffer(Enum.ReadRxBuffer.RXB1SIDH);
                }
            }

            if(rxBuffer == null)
            {
                return (false, null);
            }
                       
            RxExtendendedIdentifier8Register? eid8Register = null;
            RxExtendendedIdentifier0Register? eid0Register = null;

            var sidhRegister = new RxStandardIdentifierHighRegister(rxBuffer[0]);
            var sidlRegister = new RxStandardIdentifierLowRegister(rxBuffer[1]);

            if(sidlRegister.IDE)
            {
                eid8Register = new RxExtendendedIdentifier8Register(rxBuffer[2]);
                eid0Register = new RxExtendendedIdentifier0Register(rxBuffer[3]);
            }

            var dlcRegister = new RxDataLengthCodeRegister(rxBuffer[4]);

            var data = new byte[dlcRegister.DLC];

            Array.ConstrainedCopy(rxBuffer, 5, data, 0, dlcRegister.DLC);

            return (true, new CanMessage(sidhRegister, sidlRegister, eid8Register, eid0Register, dlcRegister, data));
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
        public Task<(bool read, CanMessage message)> ReceiveAsync(ReceieveBuffer buffer = ReceieveBuffer.Both, TimeSpan? timeout = null)
        {
            return Task.Run(() => Receive(buffer, timeout));
        }

        /// <summary>
        /// Sets the baud rate
        /// </summary>
        /// <param name="baudRate"></param>
        private void SetBaudRate(BaudRate baudRate)
        {
            WriteRegister(Registers.CNF1, (byte)(((byte)baudRate) & 0b0011_1111));
            WriteRegister(Registers.CNF2, 0b1011_0001);
            WriteRegister(Registers.CNF3, 0b0000_0101);
        }

        /// <summary>
        /// Sets the baud rate
        /// </summary>
        /// <param name="baudRate"></param>
        private Task SetBaudRateAsync(BaudRate baudRate)
        {
            return Task.Run(() => SetBaudRate(baudRate));
        }

        /// <summary>
        /// Reads the value at the given register address
        /// </summary>
        /// <param name="register">The register address</param>
        /// <returns>The read value</returns>
        public byte ReadRegister(byte register)
        {
            var readBuffer = new byte[1];
            spiDevice.TransferSequential(new[] { Commands.READ, register }, readBuffer);
            return readBuffer[0];
        }

        /// <summary>
        /// Reads the value at the given register address
        /// </summary>
        /// <param name="register">The register address</param>
        /// <returns>The read value</returns>
        public Task<byte> ReadRegisterAsync(byte register)
        {
            return Task.Run(() => ReadRegister(register));
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
            spiDevice.Write(toWrite);
        }

        /// <summary>
        /// writes the values at the starting register
        /// </summary>
        /// <param name="register">The register to start writing at</param>
        /// <param name="value">the bytes to write</param>
        public Task WriteRegisterAsync(byte register, byte[] value)
        {
            return Task.Run(() => WriteRegister(register, value));
        }

        /// <summary>
        /// Writes the given value to the specified register
        /// </summary>
        /// <param name="register">The register to write to</param>
        /// <param name="value">The value to write</param>
        public void WriteRegister(byte register, byte value)
        {
            spiDevice.Write(Commands.WRITE, register, value);
        }

        /// <summary>
        /// Writes the given value to the specified register
        /// </summary>
        /// <param name="register">The register to write to</param>
        /// <param name="value">The value to write</param>
        public Task WriteRegisterAsync(byte register, byte value)
        {
            return Task.Run(() => WriteRegister(register, value));
        }

        /// <summary>
        /// Sets the given bit of the given register  to the value specified
        /// </summary>
        /// <param name="register">The register to modify</param>
        /// <param name="bitNumber">The bit to modify</param>
        /// <param name="value">The value to set the bit to</param>
        public void WriteRegisterBit(byte register, byte bitNumber, byte value)
        {
            spiDevice.Write(Commands.BIT_MODIFY, register, ((byte)(1 << bitNumber)), (byte)(value == 0 ? 0b0000_0000 : 0b1111_1111));
        }

        /// <summary>
        /// Sets the given bit of the given register  to the value specified
        /// </summary>
        /// <param name="register">The register to modify</param>
        /// <param name="bitNumber">The bit to modify</param>
        /// <param name="value">The value to set the bit to</param>
        public Task WriteRegisterBitAsync(byte register, byte bitNumber, byte value)
        {
            return Task.Run(() => WriteRegisterBit(register, bitNumber, value));
        }

        /// <summary>
        /// sends the request to send command
        /// </summary>
        /// <param name="transmit2">Send the tx0 buffer</param>
        /// <param name="transmit1">Send the tx1 buffer</param>
        /// <param name="transmit0">Send the tx2 buffer</param>
        public void RequestToSend(bool transmit2, bool transmit1, bool transmit0)
        {
            spiDevice.Write(Commands.RTS.Set(2, transmit2).Set(1, transmit1).Set(0, transmit0));
        }

        /// <summary>
        /// sends the request to send command
        /// </summary>
        /// <param name="transmit2">Send the tx0 buffer</param>
        /// <param name="transmit1">Send the tx1 buffer</param>
        /// <param name="transmit0">Send the tx2 buffer</param>
        public Task RequestToSendAsync(bool transmit2, bool transmit1, bool transmit0)
        {
            return Task.Run(() => RequestToSend(transmit2, transmit1, transmit0));
        }

        /// <summary>
        /// Gets the status instruction response
        /// </summary>
        /// <returns>The status response</returns>
        public ReadStatusInstructionResponse ReadStatus()
        {
            var buffer = new byte[1];
            spiDevice.TransferSequential(new byte[] { Commands.READ_STATUS }, buffer);

            return new ReadStatusInstructionResponse(buffer[0]);
        }
                
        /// <summary>
        /// Gets the status instruction response
        /// </summary>
        /// <returns>The status response</returns>
        public Task<ReadStatusInstructionResponse> ReadStatusAsync()
        {
            return Task.Run(() => ReadStatus());
        }

        /// <summary>
        /// Read the full or data only buffer
        /// </summary>
        /// <param name="location">The location to read from. </param>
        /// <returns>The data read from the registers. Will read the the registers in sequence:
        /// SIDH, SIDL, EID8, EID0, DLC, D0-D7</returns>
        public byte[] ReadRxBuffer(ReadRxBuffer location)
        {
            int bufferSize = 12;
            if(location == Enum.ReadRxBuffer.RXB0DO || location == Enum.ReadRxBuffer.RXB1D0)
            {
                bufferSize = 8;
            }

            byte[] buffer = new byte[bufferSize];

            spiDevice.TransferSequential(new byte[] { (byte)location }, buffer);

            return buffer;
        }

        /// <summary>
        /// Read the full or data only buffer
        /// </summary>
        /// <param name="location">The location to read from. </param>
        /// <returns>The data read from the registers. Will read the the registers in sequence:
        /// SIDH, SIDL, EID8, EID0, DLC, D0-D7</returns>
        public Task<byte[]> ReadRxBufferAsync(ReadRxBuffer location)
        {
            return Task.Run(() => ReadRxBuffer(location));
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

            spiDevice.Write(buffer);
        }

        /// <summary>
        /// Loads the transmit buffer starting at the address indicated by teh the bufferToLoad 
        /// </summary>
        /// <param name="bufferToLoad">The transmit buffer to write to</param>
        /// <param name="values">the values to write. each byte will be written to the registers in sequence: 
        /// SIDH, SIDL, EID8, EID0, DLC, D0-D7</param>
        public Task LoadTxBufferAsync(LoadTxBuffer bufferToLoad, byte[] values)
        {
            return Task.Run(() => LoadTxBuffer(bufferToLoad, values));
        }

    }
}

