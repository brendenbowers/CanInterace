using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Windows.Devices.NetStandardWrappers.Gpio
{
    public class PinWait : ManualResetEventSlim
    {
        public EventResetMode ResetMode { get; protected set; }

        public IGpioPin GpioPin { get; protected set; }

        public GpioPinEdge SignalOnEdge { get; protected set; }

        public PinWait(IGpioPin gpioPin, bool initialState, GpioPinEdge resetOnEdge, EventResetMode resetMode = EventResetMode.AutoReset) 
            : base(initialState)
        {
            ResetMode = resetMode;
            SignalOnEdge = resetOnEdge;
            GpioPin = gpioPin ?? throw new ArgumentNullException(nameof(gpioPin));

            GpioPin.ValueChanged += OnGpioPinValueChanged;
        }

        private void OnGpioPinValueChanged(object sender, GpioPinValueChangedEventArgs e)
        {
            if(e.Edge == SignalOnEdge)
            {
                Set();
            }
            
            if(ResetMode == EventResetMode.AutoReset && e.Edge != SignalOnEdge)
            {
                Reset();
            }
        }

        protected override void Dispose(bool explicitDisposing)
        {
            var pin = GpioPin;
            GpioPin = null;
            if (pin != null)
            {
                pin.ValueChanged -= OnGpioPinValueChanged;
            }

            base.Dispose(explicitDisposing);
        }
    }
}
