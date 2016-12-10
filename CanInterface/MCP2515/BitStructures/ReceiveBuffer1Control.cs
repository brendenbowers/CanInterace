using CanInterface.Extensions;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct ReceiveBuffer1Control
    {
        /// <summary>
        /// RXM: Receive Buffer Operating Mode bits
        /// 11 = Turn mask/filters off; receive any message
        /// 10 = Receive only valid messages with extended identifiers that meet filter criteria
        /// 01 = Receive only valid messages with standard identifiers that meet filter criteria
        /// 00 = Receive all valid messages using either standard or extended identifiers that meet filter
        /// criteria
        /// </summary>
        public ReceiveBufferOperatingMode RXM;
        /// <summary>
        /// Received Remote Transfer Request bit
        /// 1 = Remote Transfer Request Received
        /// 0 = No Remote Transfer Request Received
        /// </summary>
        public bool RXRTR;
        /// <summary>
        /// Rollover Enable bit
        /// 1 = RXB0 message will rollover and be written to RXB1 if RXB0 is full
        /// 0 = Rollover disabled
        /// </summary>
        /// <summary>
        /// Filter Hit bit - indicates which acceptance filter enabled reception of message
        /// 101 = Acceptance Filter 5 (RXF5)
        /// 100 = Acceptance Filter 4 (RXF4)
        /// 011 = Acceptance Filter 3 (RXF3)
        /// 010 = Acceptance Filter 2 (RXF2)
        /// 001 = Acceptance Filter 1 (RXF1) (Only if BUKT bit set in RXB0CTRL)
        /// 000 = Acceptance Filter 0 (RXF0) (Only if BUKT bit set in RXB0CTRL)
        /// </summary>
        public FilterMatch FILHIT;

        public ReceiveBuffer1Control(byte value)
        {
            RXM = (ReceiveBufferOperatingMode)(value & (byte)ReceiveBufferOperatingMode.MASK);
            RXRTR = value.GetBit(3);
            FILHIT = (FilterMatch)(value & (byte)FilterMatch.MASK);
        }

        public ReceiveBuffer1Control(ReceiveBufferOperatingMode rxm)
        {
            RXM = rxm;
            RXRTR = false;
            FILHIT = 0;
        }

        public byte ToByte()
        {
            return (byte)RXM;
        }
    }
}
