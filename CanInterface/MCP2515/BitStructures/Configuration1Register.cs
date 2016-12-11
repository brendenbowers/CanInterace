using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct Configuration1Register
    {
        /// <summary>
        ///  Synchronization Jump Width Length bits
        /// </summary>
        public SyncronizationJumpWidth SJW;
        /// <summary>
        ///  Baud Rate Prescaler bits <5:0>
        ///  TQ = 2 x(BRP + 1)/FOSC
        /// </summary>
        public byte BRP;

        public Configuration1Register(byte value)
        {
            SJW = (SyncronizationJumpWidth)(value & (byte)SyncronizationJumpWidth.MASK);
            BRP = (byte)(value & 0b0011_1111);
        }

        public Configuration1Register(SyncronizationJumpWidth sjw, byte brp)
        {
            SJW = sjw;
            BRP = (byte)(brp & 0b0011_1111);
        }

        public byte ToByte()
        {
            return (byte)(BRP | (byte)SJW);
        }

        public static implicit operator byte(Configuration1Register register)
        {
            return register.ToByte();
        }

        public static implicit operator Configuration1Register(byte register)
        {
            return new Configuration1Register(register);
        }
    }
}
