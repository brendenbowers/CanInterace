using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    /// <summary>
    ///     Describes the possible types of change that can occur to the value of the general-purpose
    ///     I/O (GPIO) pin for the GpioPin.ValueChanged event.
    /// </summary>
    public enum GpioPinEdge
    {
        /// <summary>
        /// Summary:
        ///     The value of the GPIO pin changed from high to low.
        /// </summary>
        FallingEdge = 0,
        /// <summary>
        /// Summary:
        ///     The value of the GPIO pin changed from low to high.
        /// </summary>
        RisingEdge = 1
    }
}
