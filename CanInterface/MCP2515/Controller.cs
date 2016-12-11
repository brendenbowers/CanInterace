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
            byte sidh = 0;
            byte sidl = 0;
            byte eid8 = 0;
            byte eid0 = 0;
            byte rtr = 0;
            byte dlc = 0;
            byte ctrl = 0;
            byte intf = 0;
            byte txbd = 0;

            var status = ReadStatus();

            switch (status)
            {
                case ReadStatusInstructionResponse s when (!status.TXB0CNTRL_TXREQ && !status.CANINTF_TX0IF):
                    sidh = Registers.TXB0SIDH;
                    sidl = Registers.TXB0SIDL;
                    eid8 = Registers.TXB0EID8;
                    eid0 = Registers.TXB0EID0;
                    rtr = Registers.TXB0DLC;
                    dlc = Registers.TXB0DLC;
                    ctrl = Registers.TXB0CTRL;
                    intf = Registers.TX0IF;
                    txbd = Registers.TXB0D0;
                    break;
                case ReadStatusInstructionResponse s when (!status.TXB1CNTRL_TXREQ && !status.CANINTF_TX1IF):
                    sidh = Registers.TXB1SIDH;
                    sidl = Registers.TXB1SIDL;
                    eid8 = Registers.TXB1EID8;
                    eid0 = Registers.TXB1EID0;
                    rtr = Registers.TXB1DLC;
                    dlc = Registers.TXB1DLC;
                    ctrl = Registers.TXB1CTRL;
                    intf = Registers.TX1IF;
                    txbd = Registers.TXB1D0;
                    break;
                case ReadStatusInstructionResponse s when (!status.TXB2CNTRL_TXREQ && !status.CANINTF_TX2IF):
                    sidh = Registers.TXB2SIDH;
                    sidl = Registers.TXB2SIDL;
                    eid8 = Registers.TXB2EID8;
                    eid0 = Registers.TXB2EID0;
                    rtr = Registers.TXB2DLC;
                    dlc = Registers.TXB2DLC;
                    ctrl = Registers.TXB2CTRL;
                    intf = Registers.TX2IF;
                    txbd = Registers.TXB2D0;
                    break;
                default:
                    throw new NoAvailableBuffersException(BufferType.Transmit);
            }

            var txRegisters = message.ToTransmitRegisters();

            ///write the can message message to the various registers
            WriteRegister(sidh, txRegisters.sidh.ToByte());
            WriteRegister(sidl, txRegisters.sidl.ToByte());
            WriteRegister(eid8, message.IsExtended ? txRegisters.eid8.ToByte() : (byte)0);
            WriteRegister(eid0, message.IsExtended ? txRegisters.eid0.ToByte() : (byte)0);
            WriteRegister(dlc, txRegisters.dlc.ToByte());
            WriteRegister(txbd, txRegisters.data);

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

            byte sidh = 0;
            byte sidl = 0;
            byte eid8 = 0;
            byte eid0 = 0;
            byte rtr = 0;
            byte dlc = 0;
            byte ctrl = 0;
            byte intf = 0;
            byte rxbd = 0;

            bool isRemote = false;

            var readMessage = false;
            while(DateTime.Now < timeoutTime && !readMessage)
            {
                var interrupts = new CanInterruptFlagRegister(ReadRegister(Registers.CANINTF));
                
                if((buffer == ReceieveBuffer.RX0 || buffer == ReceieveBuffer.Both) && interrupts.RX0IF)
                {
                    sidh = Registers.RXB0SIDH;
                    sidl = Registers.RXB0SIDL;
                    eid8 = Registers.RXB0EID8;
                    eid0 = Registers.RXB0EID0;
                    rtr = Registers.RXB0DLC;
                    dlc = Registers.RXB0DLC;
                    ctrl = Registers.RXB0CTRL;
                    intf = Registers.RX0IF;
                    rxbd = Registers.RXB0D0;
                    readMessage = true;

                    isRemote = new ReceiveBuffer0Control(ReadRegister(ctrl)).RXRTR;
                }
                else if((buffer == ReceieveBuffer.RX1 || buffer == ReceieveBuffer.Both) && interrupts.RX1IF)
                {
                    sidh = Registers.RXB1SIDH;
                    sidl = Registers.RXB1SIDL;
                    eid8 = Registers.RXB1EID8;
                    eid0 = Registers.RXB1EID0;
                    rtr = Registers.RXB1DLC;
                    dlc = Registers.RXB1DLC;
                    ctrl = Registers.RXB1CTRL;
                    intf = Registers.RX1IF;
                    rxbd = Registers.RXB1D0;
                    readMessage = true;

                    isRemote = new ReceiveBuffer1Control(ReadRegister(ctrl)).RXRTR;
                }
            }

            if(!readMessage)
            {
                return (readMessage, null);
            }

           
            RxExtendendedIdentifier8Register? eid8Register = null;
            RxExtendendedIdentifier0Register? eid0Register = null;

            var sidhRegister = new RxStandardIdentifierHighRegister(ReadRegister(sidh));
            var sidlRegister = new RxStandardIdentifierLowRegister(ReadRegister(sidl));

            if(sidlRegister.IDE)
            {
                eid8Register = new RxExtendendedIdentifier8Register(ReadRegister(eid8));
                eid0Register = new RxExtendendedIdentifier0Register(ReadRegister(eid0));
            }

            var dlcRegister = new RxDataLengthCodeRegister(ReadRegister(dlc));

            var data = new byte[dlcRegister.DLC];

            spiDevice.TransferSequential(new[] { Commands.READ, rxbd }, data);

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
    }
}

