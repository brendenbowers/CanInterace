using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct ReceiveBufferPinControlAndStatusRegister
    {
        /// <summary>
        /// RX1BF Pin State bit (Digital Output mode only)
        /// - Reads as ‘0’ when RX1BF is configured as interrupt pin
        /// </summary>
        public bool B1BFS;
        /// <summary>
        ///  RX0BF Pin State bit (Digital Output mode only)
        ///  - Reads as ‘0’ when RX0BF is configured as interrupt pin
        /// </summary>
        public bool B0BFS;
        /// <summary>
        /// RX1BF Pin Function Enable bit
        /// 1 = Pin function enabled, operation mode determined by B1BFM bit
        /// 0 = Pin function disabled, pin goes to high-impedance state
        /// </summary>
        public bool B1BFE;
        /// <summary>
        ///  RX0BF Pin Function Enable bit
        ///  1 = Pin function enabled, operation mode determined by B0BFM bit
        ///  0 = Pin function disabled, pin goes to high-impedance state
        /// </summary>
        public bool B0BFE;
        /// <summary>
        /// RX1BF Pin Operation Mode bit
        ///  1 = Pin is used as interrupt when valid message loaded into RXB1
        ///  0 = Digital Output mode
        /// </summary>
        public bool B1BFM;
        /// <summary>
        /// RX0BF Pin Operation Mode bit
        /// 1 = Pin is used as interrupt when valid message loaded into RXB0
        /// 0 = Digital Output mode
        /// </summary>
        public bool B0BFM;


        public ReceiveBufferPinControlAndStatusRegister(byte value)
        {
            (_, _, B1BFS, B0BFS, B1BFE, B0BFE, B1BFM, B0BFM) = value.GetBits();
        }

        public ReceiveBufferPinControlAndStatusRegister(bool b1bfs, bool b0bfs, bool b1bfe, bool b0bfe, bool b1bfm, bool b0bfm)
        {
            B1BFS = b1bfs;
            B0BFS = b0bfs;
            B1BFE = b1bfe;
            B0BFE = b0bfe;
            B1BFM = b1bfm;
            B0BFM = b0bfm;
        }

        public byte ToByte()
        {
            return ((byte)(0b0000_0000)).SetBits(false, false, B1BFS, B0BFS, B1BFE, B0BFE, B1BFM, B0BFM);
        }

        public static implicit operator byte(ReceiveBufferPinControlAndStatusRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator ReceiveBufferPinControlAndStatusRegister(byte register)
        {
            return new ReceiveBufferPinControlAndStatusRegister(register);
        }
    }
}
