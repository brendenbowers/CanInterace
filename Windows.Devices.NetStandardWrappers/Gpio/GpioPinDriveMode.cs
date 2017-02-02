using System;
using System.Collections.Generic;
using System.Text;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    /// <summary>
    ///     Describes whether a general-purpose I/O (GPIO) pin is configured as an input
    ///     or an output, and how values are driven onto the pin.
    /// </summary>
    public enum GpioPinDriveMode
    {
        /// <summary>
        ///
        /// Summary:
        ///     Configures the GPIO pin in floating mode, with high impedance. If you call the
        ///     GpioPin.Read method for this pin, the method returns the current state of the
        ///     pin as driven externally. If you call the GpioPin.Write method, the method sets
        ///     the latched output value for the pin. The pin takes on this latched output value
        ///     when the pin is changed to an output.
        /// </summary>
        Input = 0,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin in strong drive mode, with low impedance. If you call
        ///     the GpioPin.Write method for this pin with a value of GpioPinValue.High, the
        ///     method produces a low-impedance high value for the pin. If you call the GpioPin.Write
        ///     method for this pin with a value of GpioPinValue.Low, the method produces a low-impedance
        ///     low value for the pin.If you call the GpioPin.Read method for this pin, the method
        ///     returns the value previously written to the pin.
        /// </summary>
        Output = 1,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin as high impedance with a pull-up resistor to the voltage
        ///     charge connection (VCC).If you call the GpioPin.Read method for this pin, the
        ///     method returns the value previously written to the pin.
        /// </summary>
        InputPullUp = 2,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin as high impedance with a pull-down resistor to ground.If
        ///     you call the GpioPin.Read method for this pin, the method returns the current
        ///     value of the pin as driven externally.
        /// </summary>
        InputPullDown = 3,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO in open drain mode.If you call the GpioPin.Write method for
        ///     this pin with a value of GpioPinValue.Low, the method drives a value of low to
        ///     the pin. If you call the GpioPin.Write method for this pin with a value of GpioPinValue.High,
        ///     the method places the pin in floating mode.
        /// </summary>
        OutputOpenDrain = 4,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin in open drain mode with resistive pull-up mode.If you
        ///     call the GpioPin.Write method for this pin with a value of GpioPinValue.Low,
        ///     the method produces a low-impedance low state. If you call the GpioPin.Write
        ///     method for this pin with a value of GpioPinValue.High, the method configures
        ///     the pin as high impedance with a pull-up resistor to VCC.
        /// </summary>
        OutputOpenDrainPullUp = 5,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin in open collector mode.If you call the GpioPin.Write
        ///     method for this pin with a value of GpioPinValue.High, the method drives a value
        ///     of high onto the pin. If you call the GpioPin.Write method for this pin with
        ///     a value of GpioPinValue.Low, the method configures the pin in floating mode.
        /// </summary>
        OutputOpenSource = 6,
        /// <summary>
        /// Summary:
        ///     Configures the GPIO pin in open collector mode with resistive pull-down mode.If
        ///     you call the GpioPin.Write method for this pin with a value of GpioPinValue.High,
        ///     the method drives a value of high onto the pin. If you call the GpioPin.Write
        ///     method for this pin with a value of GpioPinValue.Low, the method configures the
        ///     pin as high impedance with a pull-down resistor to ground.
        /// </summary>
        OutputOpenSourcePullDown = 7
    }
}
