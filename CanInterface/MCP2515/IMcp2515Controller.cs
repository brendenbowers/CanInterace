using System;
using System.Threading.Tasks;
using CanInterface.MCP2515.BitStructures;
using CanInterface.MCP2515.Enum;

namespace CanInterface.MCP2515
{
    public interface IMcp2515Controller : IDisposable
    {
        TimeSpan ReceiveDefaultTimeout { get; set; }
        TimeSpan ResetDefultTimeout { get; set; }
        TimeSpan TransmitDefaultTimeout { get; set; }

        void Init(BaudRate baudRate, byte frequency, SyncronizationJumpWidth syncJumpWidth);
        Task InitAsync(BaudRate baudRate, byte frequency, SyncronizationJumpWidth syncJumpWidth);
        void LoadTxBuffer(LoadTxBuffer bufferToLoad, byte[] values);
        Task LoadTxBufferAsync(LoadTxBuffer bufferToLoad, byte[] values);
        CanStatusRegister ReadCanStatus();
        Task<CanStatusRegister> ReadCanStatusAsync();
        Task<CanInterruptFlagRegister> ReadInterruptFlagsAsync();
        CanInterruptFlagRegister ReadInteruptFlags();
        byte ReadRegister(byte register);
        Task<byte> ReadRegisterAsync(byte register);
        byte[] ReadRxBuffer(ReadRxBuffer location);
        Task<byte[]> ReadRxBufferAsync(ReadRxBuffer location);
        ReadStatusInstructionResponse ReadStatus();
        Task<ReadStatusInstructionResponse> ReadStatusAsync();
        (bool read, CanMessage message) Receive(ReceiveBuffer buffer = ReceiveBuffer.Both, TimeSpan? timeout = default(TimeSpan?));
        Task<(bool read, CanMessage message)> ReceiveAsync(ReceiveBuffer buffer = ReceiveBuffer.Both, TimeSpan? timeout = default(TimeSpan?));
        void RequestToSend(bool transmit2, bool transmit1, bool transmit0);
        Task RequestToSendAsync(bool transmit2, bool transmit1, bool transmit0);
        void Reset(TimeSpan? resetTimeout = default(TimeSpan?));
        Task ResetAsync(TimeSpan? resetTimeout = default(TimeSpan?));
        void SetOperatingMode(OperatingMode mode, ReceiveBufferOperatingMode rx0BufferMode, ReceiveBufferOperatingMode rx1BufferMode, bool rollOverBuffer0To1);
        bool Transmit(CanMessage message, TimeSpan? timeout = default(TimeSpan?));
        Task<bool> TransmitAsync(CanMessage message, TimeSpan? timeout = default(TimeSpan?));
        void WriteRegister(byte register, byte[] value);
        void WriteRegister(byte register, byte value);
        Task WriteRegisterAsync(byte register, byte[] value);
        Task WriteRegisterAsync(byte register, byte value);
        void WriteRegisterBit(byte register, byte bitNumber, byte value);
        Task WriteRegisterBitAsync(byte register, byte bitNumber, byte value);
        Task SetOperatingModeAsync(OperatingMode mode, ReceiveBufferOperatingMode rx0BufferMode, ReceiveBufferOperatingMode rx1BufferMode, bool rollOverBuffer0To1);
    }
}