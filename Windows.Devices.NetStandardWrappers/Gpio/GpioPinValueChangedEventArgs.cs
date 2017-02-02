using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    /// <summary>
    ///
    /// Summary:
    ///     Provides data about the GpioPin.ValueChanged event that occurs when the value
    ///     of the general-purpose I/O (GPIO) pin changes, either because of an external
    ///     stimulus when the pin is configured as an input, or when a value is written to
    ///     the pin when the pin in configured as an output.
    /// </summary>
    public class GpioPinValueChangedEventArgs
    {
        ///<summary>
        ///
        /// Summary:
        ///     Gets the type of change that occurred to the value of the general-purpose I/O
        ///     (GPIO) pin for the GpioPin.ValueChanged event.
        ///
        /// Returns:
        ///     An enumeration value that indicates the type of change that occurred to the value
        ///     of the GPIO pin for the GpioPin.ValueChanged event.
        ///</summary>
        public GpioPinEdge Edge { get; }

        public GpioPinValueChangedEventArgs(GpioPinEdge edge)
        {
            Edge = edge;
        }
    }
}
