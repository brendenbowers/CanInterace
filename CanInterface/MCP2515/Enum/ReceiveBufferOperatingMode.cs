using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    [Flags]
    public enum ReceiveBufferOperatingMode : byte
    {
        MASK = 0b0110_0000,

        AcceptAll = 0b0110_0000,
        ExtendedIdentifierFilterMatchesOnly = 0b0100_0000,
        StandardIdentifierFilterMatchesOnly = 0b0010_0000,
        AllFilterMatches = 0b0000_0000
    }
}
