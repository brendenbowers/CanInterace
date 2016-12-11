using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct RxDataLengthCodeRegister
    {
        /// <summary>
        ///   Extended Frame Remote Transmission Request bit
        ///   (valid only when RXBnSIDL.IDE = ‘1’)
        ///   1 = Extended Frame Remote Transmit Request Received
        ///   0 = Extended Data Frame Received
        /// </summary>
        public bool RTR;
        /// <summary>
        /// Data Length Code <3:0> bits
        /// Sets the number of data bytes to be transmitted(0 to 8 bytes)
        /// Note: It is possible to set the DLC to a value greater than 8, however only 8 bytes are
        /// transmitted
        /// </summary>
        public byte DLC;

        public RxDataLengthCodeRegister(byte value)
        {
            RTR = value.GetBit(6);
            DLC = (byte)(value & 0b0000_1111);
        }

        public byte ToByte()
        {
            return DLC.Set(6, RTR);
        }

        public static implicit operator byte(RxDataLengthCodeRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator RxDataLengthCodeRegister(byte register)
        {
            return new RxDataLengthCodeRegister(register);
        }
    }
}
