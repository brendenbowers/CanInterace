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

        protected IController Controller = null;
        protected Task ReadTask = null;

        protected CancellationTokenSource ReadTaskCancellationToken = null;

        public MCP2515CanDevice(IController controller)
        {
            if(controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            ReadTaskCancellationToken = new CancellationTokenSource();
            ReadTask = new Task(ReadWorker, (ReadTaskCancellationToken.Token, controller), ReadTaskCancellationToken.Token, TaskCreationOptions.LongRunning);

        }


        protected void ReadWorker(object sync)
        {
            (CancellationToken token, IController controller) = ((CancellationToken, IController))sync;

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
            }
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
