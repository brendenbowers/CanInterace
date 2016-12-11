using CanInterface.Extensions;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct CanControlRegister
    {
        /// <summary>
        /// Request Operation Mode bits <2:0>
        /// 000 = Set Normal Operation mode
        /// 001 = Set Sleep mode
        /// 010 = Set Loopback mode
        /// 011 = Set Listen-only mode
        /// 100 = Set Configuration mode
        /// </summary>
        public OperatingMode REQOP;
        /// <summary>
        /// Abort All Pending Transmissions bit
        /// 1 = Request abort of all pending transmit buffers
        /// 0 = Terminate request to abort all transmissions
        /// </summary>
        public bool ABAT;
        /// <summary>
        ///  One Shot Mode bit
        /// 1 = Enabled.Message will only attempt to transmit one time
        /// 0 = Disabled.Messages will reattempt transmission, if required
        /// </summary>
        public bool OSM;
        /// <summary>
        ///  CLKOUT Pin Enable bit
        /// 1 = CLKOUT pin enabled
        /// 0 = CLKOUT pin disabled(Pin is in high-impedance state)
        /// </summary>
        public bool CLKEN;
        /// <summary>
        /// CLKOUT Pin Prescaler bits <1:0>
        /// 00 =FCLKOUT = System Clock/1
        /// 01 =FCLKOUT = System Clock/2
        /// 10 =FCLKOUT = System Clock/4
        /// 11 =FCLKOUT = System Clock/8
        /// </summary>
        public ClockOutPrescaler CLKPRE;
        
        public CanControlRegister(byte value)
        {
            REQOP = (OperatingMode)(value & (byte)OperatingMode.MASK);
            ABAT = value.GetBit(4);
            OSM = value.GetBit(3);
            CLKEN = value.GetBit(2);
            CLKPRE = (ClockOutPrescaler)(value & (byte)ClockOutPrescaler.MASK);
        }

        public CanControlRegister(OperatingMode reqop, bool abat, bool osm, bool clken, ClockOutPrescaler clkpre)
        {
            REQOP = reqop;
            ABAT = abat;
            OSM = osm;
            CLKEN = clken;
            CLKPRE = clkpre;
        }

        public byte ToByte()
        {
            var value = (byte)0b0000_0000;
            value |= (byte)REQOP;
            value = value.Set(4, ABAT).Set(3, OSM).Set(2, CLKEN);
            value |= (byte)CLKPRE;

            return value;
        }

        public static implicit operator byte(CanControlRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator CanControlRegister(byte register)
        {
            return new CanControlRegister(register);
        }
    }
}
