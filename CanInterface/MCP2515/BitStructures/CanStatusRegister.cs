using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct CanStatusRegister
    {
        /// <summary>
        /// The operating mode
        /// </summary>
        public OperatingMode OperatingMode;
        /// <summary>
        /// The interrupt flage code bits
        /// </summary>
        public InterruptFlagCode InterruptFlagCode;

        public CanStatusRegister(byte value)
        {
            OperatingMode = (OperatingMode)(value & (byte)OperatingMode.MASK);
            InterruptFlagCode = (InterruptFlagCode)(value & (byte)InterruptFlagCode.MASK);
        }

        public static implicit operator CanStatusRegister(byte register)
        {
            return new CanStatusRegister(register);
        }
    }
}
