using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum RecievedMessage : byte
    {
        NoMessage = 0b0000_0000,
        RXB0 = 0b0100_0000,
        RXB1 = 0b1000_0000,
        RXB1_2 = 0b1100_0000
    }
}
