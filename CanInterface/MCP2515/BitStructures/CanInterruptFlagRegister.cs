using CanInterface.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    /// <summary>
    /// 
    /// </summary>
    public struct CanInterruptFlagRegister
    {
        /// <summary>
        ///  Message Error Interrupt Flag bit
        /// </summary>
        public bool MERRF;
        /// <summary>
        /// Wakeup Interrupt Flag bit
        /// </summary>
        public bool WAKIF;
        /// <summary>
        /// Error Interrupt Flag bit (multiple sources in EFLG register)
        /// </summary>
        public bool ERRIF;
        /// <summary>
        /// Transmit Buffer 2 Empty Interrupt Flag bit
        /// </summary>
        public bool TX2IF;
        /// <summary>
        /// Transmit Buffer 1 Empty Interrupt Flag bit
        /// </summary>
        public bool TX1IF;
        /// <summary>
        /// Transmit Buffer 0 Empty Interrupt Flag bit 
        /// </summary>
        public bool TX0IF;
        /// <summary>
        /// Receive Buffer 1 Full Interrupt Flag bit
        /// </summary>
        public bool RX1IF;
        /// <summary>
        /// Receive Buffer 0 Full Interrupt Flag bit
        /// </summary>
        public bool RX0IF; 

        public CanInterruptFlagRegister(byte value)
        {
            (MERRF, WAKIF, ERRIF, TX2IF, TX1IF, TX0IF, RX1IF, RX0IF) = value.GetBits();
        }

        public CanInterruptFlagRegister(bool merrf, bool wakif, bool errif, bool tx2if, bool tx1if, bool tx0if, bool rx1if, bool rx0if)
        {
            MERRF = merrf;
            WAKIF = wakif;
            ERRIF = errif;
            TX2IF = tx2if;
            TX1IF = tx1if;
            TX0IF = tx0if;
            RX1IF = rx1if;
            RX0IF = rx0if;
        }

        public byte ToByte()
        {
            return ((byte)(0b0000_0000)).SetBits(MERRF, WAKIF, ERRIF, TX2IF, TX1IF, TX0IF, RX1IF, RX0IF);
        }

        public static implicit operator byte(CanInterruptFlagRegister register)
        {
            return register.ToByte();
        }

        public static implicit operator CanInterruptFlagRegister(byte register)
        {
            return new CanInterruptFlagRegister(register);
        }
    }
}
