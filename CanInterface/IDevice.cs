using System;
using System.Collections.Generic;
using System.Text;

namespace CanInterface
{
    public interface IDevice : IDisposable
    {
        /// <summary>
        /// Writes the bites to the device and returns the response read
        /// </summary>
        /// <param name="toWrite">The data to write</param>
        /// <param name="response">The data read after the write</param>
        void Write(byte[] toWrite, byte[] response);
        /// <summary>
        /// Writes the provided bytes to the device
        /// </summary>
        /// <param name="toWrite">The data to write</param>
        void Write(params byte[] toWrite);

    }
}
