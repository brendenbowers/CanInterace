using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.NetStandardWrappers.Spi;
using WinSpi = Windows.Devices.Spi;

namespace UwpImplementation.Windows.Devices.NetStandardWrappers.Spi
{
    public class WindowsSpiDeviceConnectionSettings : ISpiDeviceConnectionSettings
    {
        protected WinSpi.SpiConnectionSettings Settings = null;


        public WindowsSpiDeviceConnectionSettings(WinSpi.SpiConnectionSettings settings)
        {
            Settings = settings;
        }

        public int ChipSelectLine
        {
            get => Settings.ChipSelectLine;
            set => Settings.ChipSelectLine = value;
        }
        public int ClockFrequency
        {
            get => Settings.ClockFrequency;
            set => Settings.ClockFrequency = value;
        }
        public int DataBitLength
        {
            get => Settings.DataBitLength;
            set => Settings.DataBitLength = value;
        }
        public SpiMode Mode
        {
            get => (SpiMode)Settings.Mode;
            set => Settings.Mode = (WinSpi.SpiMode)value;
        }
        public SpiSharingMode SharingMode
        {
            get => (SpiSharingMode)Settings.SharingMode;
            set => Settings.SharingMode = (WinSpi.SpiSharingMode)value;
        }

        public static implicit operator WinSpi.SpiConnectionSettings(WindowsSpiDeviceConnectionSettings settings)
        {
            return settings.Settings;
        }

        public static implicit operator WindowsSpiDeviceConnectionSettings(WinSpi.SpiConnectionSettings settings)
        {
            return new WindowsSpiDeviceConnectionSettings(settings);
        }
    }
}
