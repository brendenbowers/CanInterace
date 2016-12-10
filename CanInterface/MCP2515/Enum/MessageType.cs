using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum MessageType : byte
    {
        MASK = 0b0001_1000,

        StandardData = 0b0000_0000,
        StandardRemote = 0b0000_1000,
        ExtendedData = 0b0001_0000,
        ExtendedRemote = 0b0001_1000
    }
}
