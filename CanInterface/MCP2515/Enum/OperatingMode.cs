using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum OperatingMode : byte
    {
        MASK = 0b1110_0000,

        Normal = 0b0000_0000,
        Sleep = 0b0010_0000,
        Loopback = 0b0100_0000,
        ListenOnly = 0b0110_0000,
        Configuration = 0b1000_0000
    }
}
