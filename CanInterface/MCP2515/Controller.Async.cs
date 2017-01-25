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
    public partial class Controller
    {

        public async Task InitAsync(BaudRate baudRate, byte frequency, SyncronizationJumpWidth syncJumpWidth)
        {
            await ResetAsync();
            await SetBaudRateAsync(baudRate, frequency, (byte)((byte)syncJumpWidth >> 5));
        }


        /// <summary>
        /// Readds the status from the device
        /// </summary>
        /// <returns></returns>
        public Task<CanStatusRegister> ReadCanStatusAsync()
        {
            return Task.Run(() => ReadCanStatus());
        }

        /// <summary>
        /// Resets the device
        /// </summary>
        /// <param name="resetTimeout"></param>
        /// <returns></returns>
        public Task ResetAsync(TimeSpan? resetTimeout = null)
        {
            return Task.Run(() => Reset(resetTimeout));
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
        public Task<(bool read, CanMessage message)> ReceiveAsync(ReceiveBuffer buffer = ReceiveBuffer.Both, TimeSpan? timeout = null)
        {
            return Task.Run(() => Receive(buffer, timeout));
        }
        /// <summary>
        /// Sets the baud rate
        /// </summary>
        /// <param name="baudRate"></param>
        private Task SetBaudRateAsync(BaudRate baudRate, byte frequency, byte syncJumpWidth)
        {
            return Task.Run(() => SetBaudRate(baudRate, frequency, syncJumpWidth));
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
        public Task WriteRegisterAsync(byte register, byte[] value)
        {
            return Task.Run(() => WriteRegister(register, value));
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
        public Task RequestToSendAsync(bool transmit2, bool transmit1, bool transmit0)
        {
            return Task.Run(() => RequestToSend(transmit2, transmit1, transmit0));
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
        public Task LoadTxBufferAsync(LoadTxBuffer bufferToLoad, byte[] values)
        {
            return Task.Run(() => LoadTxBuffer(bufferToLoad, values));
        }

        /// <summary>
        /// Reads the interrupt flags from the device
        /// </summary>
        /// <returns></returns>
        public Task<CanInterruptFlagRegister> ReadInterruptFlagsAsync()
        {
            return Task.Run(() => ReadInteruptFlags());
        }

        /// <summary>
        /// Sets the operating mode of the Device
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="rx0BufferMode"></param>
        /// <param name="rx1BufferMode"></param>
        /// <param name="rollOverBuffer0To1"></param>
        public Task SetOperatingModeAsync(OperatingMode mode, ReceiveBufferOperatingMode rx0BufferMode, ReceiveBufferOperatingMode rx1BufferMode, bool rollOverBuffer0To1)
        {
            return Task.Run(() => SetOperatingMode(mode, rx0BufferMode, rx1BufferMode, rollOverBuffer0To1));
        }
    }
}
