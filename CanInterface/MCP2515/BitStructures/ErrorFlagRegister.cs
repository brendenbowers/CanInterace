using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct ErrorFlagRegister
    {
        /// <summary>
        /// Receive Buffer 1 Overflow Flag bit
        /// - Set when a valid message is received for RXB1 and CANINTF.RX1IF = 1
        /// - Must be reset by MCU
        /// </summary>
        public bool RX1OVR;
        /// <summary>
        ///  Receive Buffer 0 Overflow Flag bit
        /// - Set when a valid message is received for RXB0 and CANINTF.RX0IF = 1
        /// - Must be reset by MCU
        /// </summary>
        public bool RX0OVR;
        /// <summary>
        ///  Bus-Off Error Flag bit
        /// - Bit set when TEC reaches 255
        /// - Reset after a successful bus recovery sequence
        /// </summary>
        public bool TXBO;
        /// <summary>
        ///  Transmit Error-Passive Flag bit
        /// - Set when TEC is equal to or greater than 128
        /// - Reset when TEC is less than 128
        /// </summary>
        public bool TXEP;
        /// <summary>
        ///  Receive Error-Passive Flag bit
        /// - Set when REC is equal to or greater than 128
        /// - Reset when REC is less than 128
        /// </summary>
        public bool RXEP;
        /// <summary>
        /// Transmit Error Warning Flag bit
        /// - Set when TEC is equal to or greater than 96
        /// - Reset when TEC is less than 96
        /// </summary>
        public bool TXWAR;
        /// <summary>
        /// Receive Error Warning Flag bit
        /// - Set when REC is equal to or greater than 96
        /// - Reset when REC is less than 96
        /// </summary>
        public bool RXWAR;
        /// <summary>
        ///  Error Warning Flag bit
        /// - Set when TEC or REC is equal to or greater than 96 (TXWAR or RXWAR = 1)
        /// - Reset when both REC and TEC are less than 96
        /// </summary>
        public bool EWARN;

        public ErrorFlagRegister(byte value)
        {
            (RX1OVR, RX0OVR, TXBO, TXEP, RXEP, TXWAR, RXWAR, EWARN) = value.GetBits();
        }

        public ErrorFlagRegister(bool rx1ovr, bool rx0ovr)
        {
            RX1OVR = rx1ovr;
            RX0OVR = rx0ovr;
            TXBO = false;
            TXEP = false;
            RXEP = false;
            TXWAR = false;
            RXWAR = false;
            EWARN = false;
        }

        public byte ToByte()
        {
            return ((byte)0b0000_0000).Set(7, RX1OVR).Set(6, RX0OVR);
        }

        public static implicit operator byte(ErrorFlagRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator ErrorFlagRegister(byte register)
        {
            return new ErrorFlagRegister(register);
        }
    }
}
