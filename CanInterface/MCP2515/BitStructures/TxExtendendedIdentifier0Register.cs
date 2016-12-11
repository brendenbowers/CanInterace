using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct TxExtendendedIdentifier0Register
    {
        /// <summary>
        /// The 7 to 0 bits of the Extended Identifier
        /// </summary>
        public byte EID;

        public TxExtendendedIdentifier0Register(byte value)
        {
            EID = value;
        }

        public TxExtendendedIdentifier0Register(uint id)
        {
            EID = (byte)((id << 24) >> 24);
        }

        public byte ToByte()
        {
            return EID;
        }
    }
}
