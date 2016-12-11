using CanInterface.Extensions;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct TransmitBufferControlRegister
    {
        public bool ABTF;
        public bool MLOA;
        public bool TXERR;
        public bool TXREQ;
        public TransmitBufferPriority TXP;

        public TransmitBufferControlRegister(byte value)
        {
            ABTF = value.GetBit(6);
            MLOA = value.GetBit(5);
            TXERR = value.GetBit(4);
            TXREQ = value.GetBit(3);
            TXP = (TransmitBufferPriority)(value & (byte)TransmitBufferPriority.MASK);
        }

        public TransmitBufferControlRegister(TransmitBufferPriority priority)
        {
            ABTF = false;
            MLOA = false;
            TXERR = false;
            TXREQ = false;
            TXP = priority;
        }

        public byte ToByte()
        {
            return (byte)TXP;
        }

        public static implicit operator byte(TransmitBufferControlRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator TransmitBufferControlRegister(byte register)
        {
            return new TransmitBufferControlRegister(register);
        }
    }
}
