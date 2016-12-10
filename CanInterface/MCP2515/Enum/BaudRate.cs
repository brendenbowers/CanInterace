using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    public enum BaudRate : byte
    {
        Can10K = 1,
        Can50K = 2,
        Can100K = 3,
        Can125K = 4,
        Can250K = 5,
        Can500K = 6
    }
}
