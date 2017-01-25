using System;
using System.Linq;
using System.Text;
using Windows.ApplicationModel.Background;
using WinSpi = Windows.Devices.Spi;
using System.Threading.Tasks;
using CanInterface.Uwp.Spi;
using CanInterface.Can;
using CanInterface.MCP2515.Enum;
using Windows.Storage;
using Windows.Storage.Streams;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace CanLogger
{
    public sealed class StartupTask : IBackgroundTask
    {

        private BackgroundTaskDeferral _deferral = null;
        private DataWriter _messageLog = null;
        //private NLog.Logger _logger = NLog.LogManager.GetLogger("app");

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            //_logger.Info("App Start");
            _deferral = taskInstance.GetDeferral();
            RunAsync(taskInstance).Wait();

            _messageLog?.Dispose();
            //try
            //{
            //    RunAsync(taskInstance).Wait();
            //}
            //catch(Exception ex)
            //{
            //    _logger.Error(ex, $"App Exception: {ex.Message}. Trace: {ex.StackTrace}");
            //}

            //_logger.Info("App Finish");
            
        }


        private async Task RunAsync(IBackgroundTaskInstance taskInstance)
        {

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("can.txt", CreationCollisionOption.OpenIfExists);
            _messageLog = new DataWriter(await file.OpenAsync(FileAccessMode.ReadWrite, StorageOpenOptions.None));



            var controller = await WinSpi.SpiController.GetDefaultAsync();
            
            var device = controller.GetDevice(
                new WinSpi.SpiConnectionSettings(0)
                {
                    Mode = WinSpi.SpiMode.Mode3,
                    DataBitLength = 8,
                    SharingMode = WinSpi.SpiSharingMode.Exclusive,
                    ClockFrequency= 10000000,
                });
            
            var mcp2515CanDevice = new MCP2515CanDevice(new CanInterface.MCP2515.Controller((WindowsSpiDevice)device));

            //var logger = NLog.LogManager.GetLogger("can-message");

            mcp2515CanDevice.CanMessageRecieved += (object sender, CanMessageEvent message) => {
                //logger.Info($"0x{message.Message.CanId.ToString("X4")} - {message.Message.Data.Aggregate(new StringBuilder(), (sb, b) => sb.Append(b.ToString("X2"))).ToString()}");
                var msg = $"0x{message.Message.CanId.ToString("X4")} - {message.Message.Data.Aggregate(new StringBuilder(), (sb, b) => sb.Append(b.ToString("X2"))).ToString()}";
                Console.WriteLine(msg);
                _messageLog.WriteString(msg);
            };

            //
            //mcp2515CanDevice.StartTransmitting();

            //var canMsg = new CanInterface.MCP2515.CanMessage(0x000010, false, new byte[] { 0, 1, 2, 1, 0 });

            //mcp2515CanDevice.Transmit(canMsg);
            

            //await mcp2515CanDevice.Controller.ResetAsync();
            await mcp2515CanDevice.Controller.InitAsync(BaudRate.Auto, 16, SyncronizationJumpWidth.OneXTQ);
            await mcp2515CanDevice.Controller.ResetAsync();
            await mcp2515CanDevice.Controller.SetOperatingModeAsync(OperatingMode.ListenOnly, ReceiveBufferOperatingMode.AcceptAll, ReceiveBufferOperatingMode.AcceptAll, true);


            mcp2515CanDevice.StartReceiving();

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}
