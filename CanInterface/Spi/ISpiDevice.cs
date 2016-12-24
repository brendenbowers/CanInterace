using System;
using System.Collections.Generic;
using System.Text;

namespace CanInterface.Spi
{
    public interface ISpiDevice : IDisposable
    {
        ISpiDeviceConnectionSettings ConnectionSettings { get; }
        string DeviceId { get; }

        void Read(byte[] buffer);
        void TransferFullDuplex(byte[] writeBuffer, byte[] readBuffer);
        void TransferSequential(byte[] writeBuffer, byte[] readBuffer);
        void Write(byte[] buffer);
    }
}
