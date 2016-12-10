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
        public TimeSpan TransmitDefultTimeout { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>Represent a LOW  (false) state.</summary>
        private const bool LOW = false;
        /// <summary>Represent a HIGH (true) state.</summary>
        private const bool HIGH = true;
        /// <summary></summary>
        private const byte DataIndexOffset = 2;

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

        public Task<CanStatusRegister> ReadCanStatusAsync()
        {
            return Task.Run(() => ReadCanStatus());
        }

        public CanStatusRegister ReadCanStatus()
        {
            return new CanStatusRegister(ReadRegister(Registers.CANSTAT));
        }

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
                    break;
                default:
                    throw new NoAvailableBuffersException(BufferType.Transmit);
            }



        }

        private void SetBaudRate(BaudRate baudRate)
        {
            WriteRegister(Registers.CNF1, (byte)(((byte)baudRate) & 0b0011_1111));
            WriteRegister(Registers.CNF2, 0b1011_0001);
            WriteRegister(Registers.CNF3, 0b0000_0101);
        }

        private Task SetBaudRateAsync(BaudRate baudRate)
        {
            return Task.Run(() => SetBaudRate(baudRate));
        }

        public byte ReadRegister(byte register)
        {
            var readBuffer = new byte[1];
            spiDevice.TransferSequential(new[] { Commands.READ, register }, readBuffer);
            return readBuffer[0];
        }

        public Task<byte> ReadRegisterAsync(byte register)
        {
            return Task.Run(() => ReadRegister(register));
        }

        public void WriteRegister(byte register, byte value)
        {
            spiDevice.Write(Commands.WRITE, register, value);
        }

        public Task WriteRegisterAsync(byte register, byte value)
        {
            return Task.Run(() => WriteRegister(register, value));
        }

        public void WriteRegisterByte(byte register, byte bitNumber, byte value)
        {
            spiDevice.Write(Commands.BIT_MODIFY, register, ((byte)(1 << bitNumber)), (byte)(value == 0 ? 0b0000_0000 : 0b1111_1111));
        }

        public Task WriteRegisterByteAsync(byte register, byte bitNumber, byte value)
        {
            return Task.Run(() => WriteRegisterByte(register, bitNumber, value));
        }

        public void RequestToSend(bool transmit2, bool transmit1, bool transmit0)
        {
            spiDevice.Write(Commands.RTS.Set(2, transmit2).Set(1, transmit1).Set(0, transmit0));
        }

        public Task RequestToSendAsync(bool transmit2, bool transmit1, bool transmit0)
        {
            return Task.Run(() => RequestToSend(transmit2, transmit1, transmit0));
        }

        public ReadStatusInstructionResponse ReadStatus()
        {
            var buffer = new byte[1];
            spiDevice.TransferSequential(new byte[] { Commands.READ_STATUS }, buffer);

            return new ReadStatusInstructionResponse(buffer[0]);
        }

        public Task<ReadStatusInstructionResponse> ReadStatusAsync()
        {
            return Task.Run(() => ReadStatus());
        }
    }
}

