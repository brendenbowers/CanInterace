using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum InterruptFlagCode : byte
    {
        MASK = 0b0000_1110,

        None = 0b0000_0000,
        Error  = 0b0000_0010,
        WakeUp = 0b0000_0100,
        TXB0 =  0b0000_0110,
        TXB1 = 0b0000_1000,
        TXB2 = 0b0000_1010,
        RXB0 = 0b0000_1100,
        RXB1 = 0b0000_1110,
        
    }
}
