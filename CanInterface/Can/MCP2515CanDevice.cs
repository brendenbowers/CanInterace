using CanInterface.MCP2515;
using CanInterface.MCP2515.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CanInterface.Can
{
    public class MCP2515CanDevice : IDisposable
    {
        public EventHandler<CanMessageEvent> CanMessageRecieved;

        protected Task ReadTask = null;
        protected CancellationTokenSource ReadTaskCancellationToken = null;

        public IController Controller { get; protected set; }
        public TimeSpan ReadPollingWaitPeriod { get; set; } = TimeSpan.FromMilliseconds(100);

        public MCP2515CanDevice(IController controller)
        {
            if(controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            ReadTaskCancellationToken = new CancellationTokenSource();
            ReadTask = new Task(ReadWorker, (ReadTaskCancellationToken.Token, controller, ReadPollingWaitPeriod), ReadTaskCancellationToken.Token, TaskCreationOptions.LongRunning);

        }


        protected void ReadWorker(object sync)
        {
            (CancellationToken token, IController controller, TimeSpan readWaitTime) = ((CancellationToken, IController, TimeSpan))sync;

            var wait = new ManualResetEventSlim(false);

            bool read = false;
            CanMessage message = null;
            while(!token.IsCancellationRequested)
            {
                var interrupts = controller.ReadInteruptFlags();
                
                if (interrupts.RX0IF && ((read, message) = Controller.Receive(ReceiveBuffer.RX0)).Item1)
                {
                    CanMessageRecieved?.Invoke(this, new CanMessageEvent(message, ReceiveBuffer.RX0));
                }

                if (interrupts.RX1IF && ((read, message) = Controller.Receive(ReceiveBuffer.RX1)).Item1)
                {
                    CanMessageRecieved?.Invoke(this, new CanMessageEvent(message, ReceiveBuffer.RX1));
                }

                //use a eventwait to sleep as we do not have access to thread.sleep
                wait.Wait(readWaitTime, token);
            }
        }

        public Task<bool> Transmit(CanMessage message, TimeSpan? timeout = null)
        {
            return Controller.TransmitAsync(message, timeout);
        }
        
        public void Dispose()
        {
            ReadTaskCancellationToken.Cancel();
            if(!ReadTask.Wait(TimeSpan.FromSeconds(5)))
            {
                throw new TimeoutException("Timeout waiting for read thread to shutdown");
            }
        }
        
    }
}
