using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.NetStandardWrappers.Spi;
using WinSpi = Windows.Devices.Spi;

namespace UwpImplementation.Windows.Devices.NetStandardWrappers.Spi
{
    public class WindowsSpiDevice : ISpiDevice
    {

        public WinSpi.SpiDevice SpiDevice { get; protected set; }

        public ISpiDeviceConnectionSettings ConnectionSettings { get; protected set; }

        public string DeviceId => SpiDevice.DeviceId;



        public WindowsSpiDevice(WinSpi.SpiDevice device)
        {
            SpiDevice = device ?? throw new ArgumentException(nameof(device));
            ConnectionSettings = (WindowsSpiDeviceConnectionSettings)device.ConnectionSettings;
        }

        public void Dispose()
        {
            SpiDevice?.Dispose();
        }

        public void Read(byte[] buffer)
        {
            SpiDevice.Read(buffer);
        }

        public void TransferFullDuplex(byte[] writeBuffer, byte[] readBuffer)
        {
            SpiDevice.TransferFullDuplex(writeBuffer, readBuffer);
        }

        public void TransferSequential(byte[] writeBuffer, byte[] readBuffer)
        {
            SpiDevice.TransferSequential(writeBuffer, readBuffer);
        }

        public void Write(byte[] buffer)
        {
            SpiDevice.Write(buffer);
        }

        public static implicit operator WinSpi.SpiDevice(WindowsSpiDevice device)
        {
            return device.SpiDevice;
        }

        public static implicit operator WindowsSpiDevice(WinSpi.SpiDevice device)
        {
            return new WindowsSpiDevice(device);
        }
    }
}
