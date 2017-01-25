using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    public enum BaudRate : int
    {
        Auto = 0,
        Can10K = 10,
        Can50K = 50,
        Can100K = 100,
        Can125K = 125,
        Can250K = 250,
        Can500K = 500,
        Can1M = 1000
    }
}
