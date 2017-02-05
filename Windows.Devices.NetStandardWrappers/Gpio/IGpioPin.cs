using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    public interface IGpioPin : IDisposable
    {

        event EventHandler<GpioPinValueChangedEventArgs> ValueChanged;

        bool IsDriveModeSupported(GpioPinDriveMode driveMode);
        GpioPinDriveMode GetDriveMode();
        void SetDriveMode(GpioPinDriveMode value);
        void Write(GpioPinValue value);
        GpioPinValue Read();

        TimeSpan DebounceTimeout { get; set; }
        int PinNumber { get; }
        GpioSharingMode SharingMode { get; }

    }
}
