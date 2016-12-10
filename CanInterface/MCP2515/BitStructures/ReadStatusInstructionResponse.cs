using CanInterface.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.BitStructures
{
    /// <summary>
    /// Used to quickly determine the status of the recieve and transmit buffers.
    /// </summary>
    public struct ReadStatusInstructionResponse
    {
        /// <summary>
        /// True when the Recieve buffer 0 is in the  Intterrupted (has message) state, false otherwise
        /// </summary>
        public bool CANINTF_RX0IF;
        /// <summary>
        /// True when the Recieve buffer 1 is in the  Intterrupted (has message) state, false otherwise
        /// </summary>
        public bool CANINTFL_RX1IF;
        /// <summary>
        /// The transmit buffer 0 has a pending trasmit request
        /// </summary>
        public bool TXB0CNTRL_TXREQ;
        /// <summary>
        /// The last message in the transmit buffer 0 was sent
        /// </summary>
        public bool CANINTF_TX0IF;
        /// <summary>
        /// The transmit buffer 1 has a pending trasmit request
        /// </summary>
        public bool TXB1CNTRL_TXREQ;
        /// <summary>
        /// The last message in the transmit buffer 1 was sent
        /// </summary>
        public bool CANINTF_TX1IF;
        /// <summary>
        /// The transmit buffer 2 has a pending trasmit request
        /// </summary>
        public bool TXB2CNTRL_TXREQ;
        /// <summary>
        /// The last message in the transmit buffer 2 was sent
        /// </summary>
        public bool CANINTF_TX2IF;

        public ReadStatusInstructionResponse(byte responseData)
        {
            CANINTF_RX0IF = responseData.GetBit(0);
            CANINTFL_RX1IF = responseData.GetBit(1);
            TXB0CNTRL_TXREQ = responseData.GetBit(2);
            CANINTF_TX0IF = responseData.GetBit(3);
            TXB1CNTRL_TXREQ = responseData.GetBit(4);
            CANINTF_TX1IF = responseData.GetBit(5);
            TXB2CNTRL_TXREQ = responseData.GetBit(6);
            CANINTF_TX2IF = responseData.GetBit(7);
        }
    }
}
