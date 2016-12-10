using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    public enum FilterMatch : byte
    {
        MASK    = 0b0000_0111,

        RX1RXF0 = 0b0000_0000,
        RX1RXF1 = 0b0000_0001,
        RX1RXF2 = 0b0000_0010,
        RX1RXF3 = 0b0000_0011,
        RX1RXF4 = 0b0000_0100,
        RX1RXF5 = 0b0000_0101,
        RX0RXF0 = 0b0000_0110,
        RX0RXF1 = 0b0000_0111,
    }
}
