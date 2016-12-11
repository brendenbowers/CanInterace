using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct RxStandardIdentifierHighRegister
    {
        /// <summary>
        /// 10-3 bits of the standard identifier
        /// </summary>
        public byte SIDH;

        public RxStandardIdentifierHighRegister(byte value)
        {
            SIDH = value;
        }

        public byte ToByte()
        {
            return SIDH;
        }


    }
}
