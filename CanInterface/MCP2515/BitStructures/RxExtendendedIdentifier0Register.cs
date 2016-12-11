using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct RxExtendendedIdentifier0Register
    {
        /// <summary>
        /// The 7 to 0 bits of the Extended Identifier
        /// </summary>
        public byte EID;

        public RxExtendendedIdentifier0Register(byte value)
        {
            EID = value;
        }
        
        public byte ToByte()
        {
            return EID;
        }
    }
}
