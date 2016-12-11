using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct TxStandardIdentifierLowRegister
    {
        /// <summary>
        /// Bits 2 to 0 of the Standard identifier
        /// </summary>
        public byte SIDL;
        /// <summary>
        /// Extended Identifier Enable bit
        /// 1 = Message will transmit extended identifier
        /// 0 = Message will transmit standard identifier
        /// </summary>
        public bool EXIDE;
        /// <summary>
        ///  Extended Identifier bits 17 and 16
        /// </summary>
        public byte EID;


        public TxStandardIdentifierLowRegister(byte value)
        {
            SIDL = (byte)(value >> 5);
            EXIDE = value.GetBit(3);
            EID = (byte)(value & 0b0000_0011);
        }

        public TxStandardIdentifierLowRegister(uint id, bool extendend)
        {
            SIDL = (byte)(((id << 11) >> 24) & 0b1110_000);
            EXIDE = extendend;

            if(extendend)
            {
                EID = (byte)(((id << 14) >> 30) & 0b0000_0011);
            }
            else
            {
                EID = 0;
            }
        }

        public byte ToByte()
        {
            return ((byte)(SIDL | EID)).Set(3, EXIDE);
        }


        public static implicit operator byte(TxStandardIdentifierLowRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator TxStandardIdentifierLowRegister(byte register)
        {
            return new TxStandardIdentifierLowRegister(register);
        }
    }
}
