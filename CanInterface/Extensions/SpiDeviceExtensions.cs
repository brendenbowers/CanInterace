using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanInterface.Extensions
{
    public static class SpiDeviceExtensions
    {
        public static void Write(this Spi.ISpiDevice device, params byte[] data)
        {
            device.Write(data);
        }
    }
}
