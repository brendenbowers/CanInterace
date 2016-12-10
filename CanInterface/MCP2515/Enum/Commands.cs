using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Enum
{
    public static class Commands
    {
        public static byte RESET = 0b1100_0000;
        public static byte READ = 0b0000_0011;
        public static byte READ_RXB0SIDH = 0b1001_0000;
        public static byte READ_RXB0D0 = 0b1001_0010;
        public static byte READ_RXB1SIDH = 0b1001_0100;
        public static byte READ_RXB1D0 = 0b1001_0110;
        public static byte WRITE = 0b0000_0010;
        public static byte LOAD_TXB0SIDH = 0b0100_0000;
        public static byte LOAD_TXB0D0 = 0b0100_0001;
        public static byte LOAD_TXB1SIDH = 0b0100_0010;
        public static byte LOAD_TXB1D0 = 0b0100_0011;
        public static byte LOAD_TXB2SIDH = 0b0100_0100;
        public static byte LOAD_TXB2D0 = 0b0100_0101;
        public static byte RTS = 0b1000_0000;
        public static byte READ_STATUS = 0b1010_0000;
        public static byte RX_STATUS = 0b1011_0000;
        public static byte BIT_MODIFY = 0b0000_0101;
    }
}
