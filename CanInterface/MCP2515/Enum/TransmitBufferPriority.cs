using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum TransmitBufferPriority : byte
    {
        MASK = 0b0000_0011,
        High = 0b0000_0011,
        HighIntermidate = 0b0000_0010,
        LowIntermidate = 0b0000_0001,
        Low = 0b0000_0000,
    }
}
