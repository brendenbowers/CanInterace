using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{

    public struct CanInterruptEnableRegister
    {
        /// <summary>
        ///  Message Error Interrupt Enable bit
        /// </summary>
        public bool MERRE;
        /// <summary>
        /// Wakeup Interrupt Enable bit
        /// </summary>
        public bool WAKIE;
        /// <summary>
        /// Error Interrupt Enable bit (multiple sources in EFLG register)
        /// </summary>
        public bool ERRIE;
        /// <summary>
        /// Transmit Buffer 2 Empty Interrupt Enable bit
        /// </summary>
        public bool TX2IE;
        /// <summary>
        /// Transmit Buffer 1 Empty Interrupt Enable bit
        /// </summary>
        public bool TX1IE;
        /// <summary>
        /// Transmit Buffer 0 Empty Interrupt Enable bit 
        /// </summary>
        public bool TX0IE;
        /// <summary>
        /// Receive Buffer 1 Full Interrupt Enable bit
        /// </summary>
        public bool RX1IE;
        /// <summary>
        /// Receive Buffer 0 Full Interrupt Enable bit
        /// </summary>
        public bool RX0IE;

        public CanInterruptEnableRegister(byte value)
        {
            (MERRE, WAKIE, ERRIE, TX2IE, TX1IE, TX0IE, RX1IE, RX0IE) = value.GetBits();
        }

        public CanInterruptEnableRegister(bool merre, bool wakie, bool errie, bool tx2ie, bool tx1ie, bool tx0ie, bool rx1ie, bool rx0ie)
        {
            MERRE = merre;
            WAKIE = wakie;
            ERRIE = errie;
            TX2IE = tx2ie;
            TX1IE = tx1ie;
            TX0IE = tx0ie;
            RX1IE = rx1ie;
            RX0IE = rx0ie;
        }

        public byte ToByte()
        {
            return ((byte)(0b0000_0000)).SetBits(MERRE, WAKIE, ERRIE, TX2IE, TX1IE, TX0IE, RX1IE, RX0IE);
        }
        public static implicit operator byte(CanInterruptEnableRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator CanInterruptEnableRegister(byte register)
        {
            return new CanInterruptEnableRegister(register);
        }
    }
}
