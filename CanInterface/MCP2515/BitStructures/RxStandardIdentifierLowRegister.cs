using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct RxStandardIdentifierLowRegister
    {
        /// <summary>
        /// Bits 2 to 0 of the Standard identifier
        /// </summary>
        public byte SIDL;
        /// <summary>
        /// Standard Frame Remote Transmit Request bit (valid only if IDE bit = ‘0’)
        /// 1 = Standard Frame Remote Transmit Request Received
        /// 0 = Standard Data Frame Received
        /// </summary>
        public bool SRR;
        /// <summary>
        /// Extended Identifier Flag bit
        /// 1 = Received message was an Extended Frame
        /// 0 = Received message was a Standard Frame
        /// </summary>
        public bool IDE;
        /// <summary>
        ///  Extended Identifier bits 17 and 16
        /// </summary>
        public byte EID;


        public RxStandardIdentifierLowRegister(byte value)
        {
            SIDL = (byte)(value >> 5);
            SRR = value.GetBit(4);
            IDE = value.GetBit(3);
            EID = (byte)(value & 0b0000_0011);
        }

        public static implicit operator RxStandardIdentifierLowRegister(byte register)
        {
            return new RxStandardIdentifierLowRegister(register);
        }
    }
}
