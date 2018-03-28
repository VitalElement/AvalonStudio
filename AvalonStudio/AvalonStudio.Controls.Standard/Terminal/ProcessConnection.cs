using Avalonia.Threading;
using AvalonStudio.Platforms.Terminals;
using System;
using System.Threading;
using System.Threading.Tasks;
using VtNetCore.Avalonia;

namespace AvalonStudio.Controls.Standard.Terminal
{
    class PsuedoTerminalConnection : IConnection, IDisposable
    {
        private CancellationTokenSource _cancellationSource;

        private bool _isConnected = false;

        IPsuedoTerminal _terminal;

        public PsuedoTerminalConnection(IPsuedoTerminal terminal)
        {
            _terminal = terminal;
        }

        public bool IsConnected => _isConnected;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public event EventHandler<EventArgs> Closed;

        public bool Connect()
        {
            _cancellationSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                var data = new byte[4096];

                while (!_cancellationSource.IsCancellationRequested)
                {
                    var bytesReceived = await _terminal.ReadAsync(data, 0, data.Length);

                    if (bytesReceived > 0)
                    {
                        var receviedData = new byte[bytesReceived];

                        Buffer.BlockCopy(data, 0, receviedData, 0, bytesReceived);

                        await Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            DataReceived?.Invoke(this, new DataReceivedEventArgs { Data = receviedData });
                        });
                    }

                    await Task.Delay(5);
                }
            }, _cancellationSource.Token);

            _isConnected = true;

            _terminal.Process.EnableRaisingEvents = true;
            _terminal.Process.Exited += Process_Exited;

            return _isConnected;
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            _terminal.Process.Exited -= Process_Exited;

            Closed?.Invoke(this, EventArgs.Empty);            
        }

        public void Disconnect()
        {
            _cancellationSource.Cancel();
            _terminal.Dispose();
        }

        public void SendData(byte[] data)
        {
            _terminal.WriteAsync(data, 0, data.Length).Wait();
        }

        public void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {
            _terminal?.SetSize(columns, rows);
        }

        public void Dispose()
        {
            _cancellationSource?.Cancel();

            Disconnect();
        }
    }
}
