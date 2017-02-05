using System;
using Windows.Devices.NetStandardWrappers.Gpio;
using WinGpio = Windows.Devices.Gpio;

namespace UwpImplementation.Windows.Devices.NetStandardWrappers.Gpio
{
    public class WindowsGpioPin : IGpioPin
    {

        public WinGpio.GpioPin GpioPin { get; protected set; }
                
        public TimeSpan DebounceTimeout { get => GpioPin.DebounceTimeout; set => GpioPin.DebounceTimeout = value; }

        public int PinNumber => GpioPin.PinNumber;

        public GpioSharingMode SharingMode => (GpioSharingMode)(int)GpioPin.SharingMode;

        public event EventHandler<GpioPinValueChangedEventArgs> ValueChanged;


        public WindowsGpioPin(WinGpio.GpioPin gpioPin)
        {
            GpioPin = gpioPin ?? throw new ArgumentNullException(nameof(gpioPin));
            GpioPin.ValueChanged += OnGpioPinValueChanged;
        }
        
        public GpioPinDriveMode GetDriveMode() => (GpioPinDriveMode)(int)GpioPin.GetDriveMode();

        public bool IsDriveModeSupported(GpioPinDriveMode driveMode) => GpioPin.IsDriveModeSupported((WinGpio.GpioPinDriveMode)(int)driveMode);


        public GpioPinValue Read() => (GpioPinValue)(int)GpioPin.Read();
        

        public void SetDriveMode(GpioPinDriveMode driveMode) => GpioPin.SetDriveMode((WinGpio.GpioPinDriveMode)(int)driveMode);

        public void Write(GpioPinValue pinValue) => GpioPin.Write((WinGpio.GpioPinValue)(int)pinValue);

        private void OnGpioPinValueChanged(WinGpio.GpioPin sender, WinGpio.GpioPinValueChangedEventArgs args)
        {
            ValueChanged?.Invoke(sender, new GpioPinValueChangedEventArgs((GpioPinEdge)(int)args.Edge));
        }

        public void Dispose()
        {
            GpioPin.Dispose();
        }

        public static implicit operator WinGpio.GpioPin(WindowsGpioPin gpioPin)
        {
            return gpioPin.GpioPin;
        }

        public static implicit operator WindowsGpioPin(WinGpio.GpioPin gpioPin)
        {
            return new WindowsGpioPin(gpioPin);
        }
    }
}
