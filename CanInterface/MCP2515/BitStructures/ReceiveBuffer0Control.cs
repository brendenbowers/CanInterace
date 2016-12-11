using CanInterface.Extensions;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    public struct ReceiveBuffer0Control
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
        public bool BUKT;
        /// <summary>
        /// Read-only Copy of BUKT bit (used internally by the MCP2515)
        /// </summary>
        public bool BUKT1;
        /// <summary>
        /// T: Filter Hit bit - indicates which acceptance filter enabled reception of message
        /// 1 = Acceptance Filter 1 (RXF1)
        /// 0 = Acceptance Filter 0 (RXF0)
        /// 
        /// Note: If a rollover from RXB0 to RXB1 occurs, the FILHIT bit will reflect the filter that accepted
        /// the message that rolled over
        /// </summary>
        public bool FILHIT;

        public ReceiveBuffer0Control(byte value)
        {
            RXM = (ReceiveBufferOperatingMode)(value & (byte)ReceiveBufferOperatingMode.MASK);
            RXRTR = value.GetBit(3);
            BUKT = value.GetBit(2);
            BUKT1 = value.GetBit(1);
            FILHIT = value.GetBit(0);
        }

        public ReceiveBuffer0Control(ReceiveBufferOperatingMode rxm, bool bukt)
        {
            RXM = rxm;
            RXRTR = false;
            BUKT = bukt;
            BUKT1 = false;
            FILHIT = false;
        }

        public byte ToByte()
        {
            return ((byte)RXM).Set(2, BUKT);
        }
        
        public static implicit operator byte(ReceiveBuffer0Control register)
        {
            return register.ToByte();
        }

        public static implicit operator ReceiveBuffer0Control(byte register)
        {
            return new ReceiveBuffer0Control(register);
        }
    }
}
