using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515
{
    public class CanMessage
    {
        private const int CANID_11BITS = 0x7FF;

        public uint CanId { get; set; }
        public bool IsExtended => CanId > CANID_11BITS;
        public bool IsRemote;
        public byte[] Data { get; set; } = new byte[8];

    }
}
