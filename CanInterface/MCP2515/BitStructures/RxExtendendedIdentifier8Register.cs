using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct RxExtendendedIdentifier8Register
    {
        /// <summary>
        /// The 15 to 8 bits of the Extended Identifier
        /// </summary>
        public byte EID;

        public RxExtendendedIdentifier8Register(byte value)
        {
            EID = value;
        }
        public byte ToByte()
        {
            return EID;
        }

        public static implicit operator byte(RxExtendendedIdentifier8Register register)
        {
            return register.ToByte();
        }

        public static implicit operator RxExtendendedIdentifier8Register(byte register)
        {
            return new RxExtendendedIdentifier8Register(register);
        }
    }
}
