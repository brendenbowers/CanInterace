using System;
using System.Collections.Generic;
using System.Text;

namespace CanInterface.Spi
{
    public interface ISpiDeviceConnectionSettings
    {
        int ChipSelectLine { get; set; }
        int ClockFrequency { get; set; }
        int DataBitLength { get; set; }
        SpiMode Mode { get; set; }
        SpiSharingMode SharingMode { get; set; }
    }
}
