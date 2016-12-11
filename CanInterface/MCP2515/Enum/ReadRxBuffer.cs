using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum ReadRxBuffer : byte
    {
        RXB0SIDH = 0b1001_0000,
        RXB0DO = 0b1001_0010,
        RXB1SIDH = 0b1001_0100,
        RXB1D0 = 0b1001_0110,
    }
}
