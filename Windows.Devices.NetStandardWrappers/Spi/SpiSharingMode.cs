using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Spi
{
    //
    // Summary:
    //     Defines the sharing mode for the SPI bus.
    public enum SpiSharingMode
    {
        //
        // Summary:
        //     SPI bus segment is not shared.
        Exclusive = 0,
        //
        // Summary:
        //     SPI bus is shared.
        Shared = 1
    }
}
