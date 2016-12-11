using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct TxExtendendedIdentifier8Register
    {
        /// <summary>
        /// The 15 to 8 bits of the Extended Identifier
        /// </summary>
        public byte EID;

        public TxExtendendedIdentifier8Register(byte value)
        {
            EID = value;
        }

        public TxExtendendedIdentifier8Register(uint id)
        {
            EID = (byte)((id << 16) >> 24);
        }

        public byte ToByte()
        {
            return EID;
        }

        public static implicit operator byte(TxExtendendedIdentifier8Register register)
        {
            return register.ToByte();
        }

        public static implicit operator TxExtendendedIdentifier8Register(byte register)
        {
            return new TxExtendendedIdentifier8Register(register);
        }
    }
}
