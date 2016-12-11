using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Exceptions
{
    public class NoAvailableBuffersException : Exception
    {
        public BufferType BufferType { get; set; }

        public NoAvailableBuffersException(string message, BufferType bufferType) : base(message)
        {
            BufferType = bufferType;
        }

        public NoAvailableBuffersException(BufferType bufferType) 
            : this($"No {bufferType} buffers are available", bufferType)
        {
        }
    }
}
