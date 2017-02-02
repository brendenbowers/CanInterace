using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.NetStandardWrappers.Spi;

namespace CanInterface.Extensions
{
    public static class SpiDeviceExtensions
    {
        public static void Write(this ISpiDevice device, params byte[] data)
        {
            device.Write(data);
        }
    }
}
