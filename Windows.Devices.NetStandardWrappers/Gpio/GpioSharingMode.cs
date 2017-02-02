using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    /// <summary>
    ///     Describes the modes in which you can open a general-purpose I/O (GPIO) pin. These
    ///     modes determine whether other connections to the GPIO pin can be opened while
    ///     you have the pin open.
    /// </summary>
    public enum GpioSharingMode
    {
        /// <summary>
        ///    Opens the GPIO pin exclusively, so that no other connection to the pin can be
        ///     opened.
        /// </summary>
        Exclusive = 0,
        /// <summary>
        ///     Opens the GPIO pin as shared, so that other connections in SharedReadOnly mode
        ///     to the pin can be opened. You can only perform operations that do not change
        ///     the state of the GPIO pin in shared mode. Operations that you can perform on
        ///     the GPIO pin in shared mode include:Calling the GpioPin.Read method.Calling the
        ///     GpioPin.GetDriveMode method.Getting the values of properties, such as GpioPin.PinNumber
        ///     and GpioPin.DebounceTimeout.Registering an event handler for the GpioPin.ValueChanged
        ///     event.
        /// </summary>
        SharedReadOnly = 1
    }
}
