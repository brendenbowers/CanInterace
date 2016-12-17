using CanInterface.MCP2515;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Can
{
    public class CanMessageEvent : EventArgs
    {
        public CanMessage Message { get; protected set; }
        public ReceiveBuffer ReceivedFrom { get; protected set; }

        public CanMessageEvent(CanMessage message, ReceiveBuffer receiveBuffer)
        {
            Message = message;
            ReceivedFrom = receiveBuffer;
        }
    }
}
