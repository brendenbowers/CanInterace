using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Spi
{
    public enum SpiMode
    {
        //
        // Summary:
        //     CPOL = 0, CPHA = 0.
        Mode0 = 0,
        //
        // Summary:
        //     CPOL = 0, CPHA = 1.
        Mode1 = 1,
        //
        // Summary:
        //     CPOL = 1, CPHA = 0.
        Mode2 = 2,
        //
        // Summary:
        //     CPOL = 1, CPHA = 1.
        Mode3 = 3
    }
}
