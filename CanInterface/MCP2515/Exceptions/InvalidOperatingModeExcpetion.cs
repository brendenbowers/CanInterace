using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.MCP2515.Exceptions
{
    public class InvalidOperatingModeExcpetion : Exception
    {
        /// <summary>
        /// The requested operating mode at the time of the excepton
        /// </summary>
        public OperatingMode Requested { get; set; }
        /// <summary>
        /// The operating mode that the device is in
        /// </summary>
        public OperatingMode Actual { get; set; }

        public InvalidOperatingModeExcpetion(OperatingMode requested, OperatingMode actual, string message, Exception innerException) 
            :base(message, innerException)
        {
            Requested = requested;
            Actual = actual;
        }


        public InvalidOperatingModeExcpetion(OperatingMode requested, OperatingMode actual, string message)
            : this(requested, actual, message, null)
        {
        }

        public InvalidOperatingModeExcpetion(OperatingMode requested, OperatingMode actual)
            : this(requested, actual, $"The requested operating mode {requested} did not match the operating mode of the device {actual}", null)
        {
        }
    }
}
