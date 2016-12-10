using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Extensions
{
    public static class ByteExtensions
    {
        /// <summary>
        /// Gets the bit value at the given index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static bool GetBit(this byte value, int offset)
        {
            if(offset < 0 || offset > 7)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), $"{nameof(offset)} must be between 1 and 8");
            }
            return (value & (1 << offset)) != 0;
        }

        public static byte GetRightBits(this byte value, int numBits)
        {
            return (byte)(value & ((1 << numBits) - 1));
        }

        public static byte GetLeftBits(this byte value, int numBits)
        {
            return (byte)((value >> (8 - numBits)) << (8 - numBits));
        }

        public static byte GetMidBits(this byte value, int start, int end)
        {
            //return value.GetLeftBits(8 - start).GetRightBits(8 - end);
            return (byte)(value & ((~(~0 << (end - start + 1))) << end));
        }
        /// <summary>
        /// Gets all the bits in a byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static (bool bit7, bool bit6, bool bit5, bool bit4, bool bit3, bool bit2, bool bit1, bool bit0) GetBits(this byte value)
        {

            return ((value & (1 << 7)) != 0, (value & (1 << 6)) != 0, (value & (1 << 5)) != 0, (value & (1 << 4)) != 0, 
                (value & (1 << 3)) != 0, (value & (1 << 2)) != 0, (value & (1 << 1)) != 0, (value & (1 << 0)) != 0);
        }

        public static byte Set(this byte value, byte position)
        {
            return (byte)(value | (0b0000_0001 << position));
        }

        public static byte Set(this byte value, byte position, bool on)
        {
            return on ? Set(value, position) : Clear(value, position);
        }

        public static byte Clear(this byte value, byte position)
        {
            return (byte)(value & ~(0b0000_0001 << position));
        }

        public static byte SetBits(this byte value, bool bit7, bool bit6, bool bit5, bool bit4, bool bit3, bool bit2, bool bit1, bool bit0)
        {
            return ((byte)(0b0000_0000)).Set(7, bit7).Set(6, bit6).Set(5, bit5)
                .Set(4, bit4).Set(3, bit3).Set(2, bit2).Set(1, bit1).Set(0, bit0);
        }

        public static T ToEnumFromMask<T>(this byte value, byte mask)
            where T : struct
        {
           return (T)(object)(value & mask);
        }

        
    }
}
