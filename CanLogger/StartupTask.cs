using System;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using WinSpi = Windows.Devices.Spi;
using WinGpio = Windows.Devices.Gpio;
using System.Threading.Tasks;
using CanInterface.Can;
using CanInterface.MCP2515.Enum;
using Windows.Storage;
using Windows.Storage.Streams;
using UwpImplementation.Windows.Devices.NetStandardWrappers.Spi;
using UwpImplementation.Windows.Devices.NetStandardWrappers.Gpio;
using System.Threading;
using System.Diagnostics;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace CanLogger
{
    public sealed class StartupTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var deferral = taskInstance.GetDeferral();
            Task.Run(() => RunAsync(taskInstance, deferral));
        }


        private async void RunAsync(IBackgroundTaskInstance taskInstance, BackgroundTaskDeferral deferral)
        {
            
            var waitForCancel = new ManualResetEventSlim(false);

            taskInstance.Canceled += (IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason) => {
                Debug.Write($"Background task cancelled: {reason}");
                waitForCancel.Set();
            };

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("can.txt", CreationCollisionOption.OpenIfExists);
            using (var messageLog = new DataWriter(await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None)))
            {
                var gpio = await WinGpio.GpioController.GetDefaultAsync();
                if (!gpio.TryOpenPin(17, WinGpio.GpioSharingMode.Exclusive, out var interruptPin, out var status))
                {
                    Debug.WriteLine($"Pin 17 failed to open. Status: {status}");
                    return;
                }

                using (interruptPin)
                {
                    interruptPin.SetDriveMode(WinGpio.GpioPinDriveMode.InputPullDown);
                    
                    var controller = await WinSpi.SpiController.GetDefaultAsync();

                    using (var device = controller.GetDevice(
                        new WinSpi.SpiConnectionSettings(0)
                        {
                            Mode = WinSpi.SpiMode.Mode3,
                            DataBitLength = 8,
                            SharingMode = WinSpi.SpiSharingMode.Exclusive,
                            ClockFrequency = 10000000,
                        }))
                    {

                        using (var canDevice = new CanDevice(new CanInterface.MCP2515.Mcp2515Controller((WindowsSpiDevice)device), (WindowsGpioPin)interruptPin, Windows.Devices.NetStandardWrappers.Gpio.GpioPinEdge.FallingEdge))
                        {

                            canDevice.CanMessageRecieved += async (object sender, CanMessageEvent message) =>
                            {
                                var msg = $"0x{message.Message.CanId.ToString("X4")} - {message.Message.Data.Aggregate(new StringBuilder(), (sb, b) => sb.Append($"0x{b.ToString("X2")} ")).ToString()}";
                                Debug.WriteLine($"Can Message: {msg}");
                                messageLog.WriteString(msg);
                                await messageLog.StoreAsync();
                            };
                            
                            await canDevice.Controller.InitAsync(BaudRate.Can500K, 16, SyncronizationJumpWidth.OneXTQ);
                            await canDevice.Controller.SetOperatingModeAsync(OperatingMode.Normal, ReceiveBufferOperatingMode.AcceptAll, ReceiveBufferOperatingMode.AcceptAll, true);
                            
                            canDevice.StartReceiving();
                            waitForCancel.Wait();
                        }
                    }

                    await messageLog?.StoreAsync();
                    deferral.Complete();
                }

            }
        }
    }
}
