using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct TxStandardIdentifierHighRegister
    {
        /// <summary>
        /// 10-3 bits of the standard identifier
        /// </summary>
        public byte SIDH;

        public TxStandardIdentifierHighRegister(byte value)
        {
            SIDH = value;
        }

        /// <summary>
        /// The address to load the identifier from
        /// </summary>
        /// <param name="address"></param>
        public TxStandardIdentifierHighRegister(uint address)
        {
            //gets the 10th to 3rd bit of the standard identifier
            // drop 16 of the Extended bits and then 5 of the matched bits
            SIDH = (byte)((uint)((address >> 16) & 0xFFFF) >> 5);
        }

        public byte ToByte()
        {
            return SIDH;
        }

        public static implicit operator byte(TxStandardIdentifierHighRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator TxStandardIdentifierHighRegister(byte register)
        {
            return new TxStandardIdentifierHighRegister(register);
        }


    }
}
