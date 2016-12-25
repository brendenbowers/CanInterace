using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum SyncronizationJumpWidth : byte
    {
        MASK = 0b1100_0000,

        FourXTQ = 0b1100_0000,
        ThreeXTQ = 0b1000_0000,
        TwoXTQ = 0b0100_0000,
        OneXTQ = 0b0000_0000,         
    }
}
