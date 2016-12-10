using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public class Configuration2Register
    {
        /// <summary>
        /// PS2 Bit Time Length bit
        /// 1 = Length of PS2 determined by PHSEG22:PHSEG20 bits of CNF3
        /// 0 = Length of PS2 is the greater of PS1 and IPT(2 TQ)
        /// </summary>
        public bool BTLMODE;
        /// <summary>
        ///  Sample Point Configuration bit
        /// 1 = Bus line is sampled three times at the sample point
        /// 0 = Bus line is sampled once at the sample point
        /// </summary>
        public bool SAM;
        /// <summary>
        ///  PS1 Length bits<2:0>
        ///  (PHSEG1 + 1) x TQ
        /// </summary>
        public byte PHSEG1;
        /// <summary>
        ///  Propagation Segment Length bits <2:0>
        ///  (PRSEG + 1) x TQ
        /// </summary>
        public byte PRSEG;

        public Configuration2Register(byte value)
        {
            BTLMODE = value.GetBit(7);
            SAM = value.GetBit(6);
            PHSEG1 = (byte)(value & 0b0011_1000);
            PRSEG = (byte)(value & 0b0000_0111);
        }

        public Configuration2Register(bool btlmode, bool sam, byte phseg1, byte prseg)
        {
            BTLMODE = btlmode;
            SAM = sam;
            PHSEG1 = (byte)(phseg1 & 0b0011_1000);
            PRSEG = (byte)(prseg & 0b0000_0111);
        }

        public byte ToByte()
        {
            byte value = 0b0000_0000;
            value = value.Set(7, BTLMODE).Set(6, SAM);
            value |= PHSEG1;
            value |= PRSEG;
            return value;
        }
    }
}
