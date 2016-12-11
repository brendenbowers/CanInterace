using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum LoadTxBuffer : byte
    {
        TX0SIDH = 0b0100_0000,
        TX0D0 = 0b0100_00010,
        TX1SIDH = 0b0100_00100,
        TX1D0 = 0b0100_00110,
        TX2SIDH = 0b0100_01000,
        TX2D0 = 0b0100_01010
    }
}
