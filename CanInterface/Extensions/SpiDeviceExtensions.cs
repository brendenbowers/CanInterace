using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Spi;

namespace CanInterface.Extensions
{
    public static class SpiDeviceExtensions
    {
        public static void Write(this SpiDevice device, params byte[] data)
        {
            device.Write(data);
        }
    }
}
