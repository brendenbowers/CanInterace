using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Exceptions
{
    public class InvalidConfigurationException : Exception
    {
        public InvalidConfigurationException(string message) 
            : base(message)
        {
        }

        public InvalidConfigurationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
