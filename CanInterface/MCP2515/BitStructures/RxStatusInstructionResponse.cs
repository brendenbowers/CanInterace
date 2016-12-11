using CanInterface.Extensions;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    /// <summary>
    /// Used to quickly determine what filter caused a message to be palced in a buffer.
    /// </summary>
    public struct RxStatusInstructionResponse
    {
        /// <summary>
        /// True if the receive buffer 0 has a message
        /// </summary>
        public bool RXB0HasMessage;
        /// <summary>
        /// True if the recieve buffer 1 has a message
        /// </summary>
        public bool RXB1HasMessage;
        /// <summary>
        /// The type of message received
        /// </summary>
        public MessageType MessageType;
        /// <summary>
        /// The buffer that matched the message
        /// </summary>
        public FilterMatch FilterMatch;

        public RxStatusInstructionResponse(byte value)
        {
            RXB0HasMessage = value.GetBit(7);
            RXB1HasMessage = value.GetBit(6);
            MessageType = (MessageType)(value & (byte)MessageType.MASK);
            FilterMatch = (FilterMatch)(value & (byte)FilterMatch.MASK);
        }

        public static implicit operator RxStatusInstructionResponse(byte register)
        {
            return new RxStatusInstructionResponse(register);
        }
    }
}
