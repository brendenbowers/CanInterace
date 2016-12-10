using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    public enum ClockOutPrescaler : byte
    {
        MASK = 0b0000_0011,

        One = 0b0000_0000,
        Two = 0b0000_0001,
        Three = 0b0000_0010,
        Four = 0b0000_0011
    }
}
