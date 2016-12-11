using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct Configuration3Register
    {
        /// <summary>
        /// Start-of-Frame signal bit
        ///If CANCTRL.CLKEN = 1:
        ///1 = CLKOUT pin enabled for SOF signal
        ///0 = CLKOUT pin enabled for clockout function
        ///If CANCTRL.CLKEN = 0, Bit is don’t care.
        /// </summary>
        public bool SOF;
        /// <summary>
        /// Wake-up Filter bit
        /// 1 = Wake-up filter enabled
        /// 0 = Wake-up filter disabled
        /// </summary>
        public bool WAKFIL;
        /// <summary>
        /// PS2 Length bits<2:0>
        ///         (PHSEG2 + 1) x TQ
        ///         Note: Minimum valid setting for PS2 is 2 TQ
        /// </summary>
        public byte PHSEG2;

        public Configuration3Register(byte value)
        {
            SOF = value.GetBit(7);
            WAKFIL = value.GetBit(6);
            PHSEG2 = (byte)(value & 0b0000_0111);
        }

        public Configuration3Register(bool sof, bool wakfil, byte phseg2)
        {
            SOF = sof;
            WAKFIL = wakfil;
            PHSEG2 = (byte)(phseg2 & 0b0000_0111);
        }

        public byte ToByte()
        {
            return PHSEG2.Set(7, SOF).Set(6, WAKFIL);
        }

        public static implicit operator byte(Configuration3Register register)
        {
            return register.ToByte();
        }

        public static implicit operator Configuration3Register(byte register)
        {
            return new Configuration3Register(register);
        }
    }
}
