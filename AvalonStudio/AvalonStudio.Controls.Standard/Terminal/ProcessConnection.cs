using System;
using System.Collections.Generic;
using System.Text;
using AvalonStudio.Platforms;
using VtNetCore.Avalonia;

namespace AvalonStudio.Controls.Standard.Terminal
{
    class ProcessConnection : IConnection
    {
        private ProcessRunner _runner;
        public ProcessConnection(ProcessRunner runner)
        {
            _runner = runner;
            _runner.StdErrorData.Subscribe(data =>
            {
                DataReceived(this, new DataReceivedEventArgs { Data = data });
            });

            _runner.StdOutData.Subscribe(data =>
            {
                DataReceived(this, new DataReceivedEventArgs { Data = data });
            });
        }
        public bool IsConnected => _runner.Started && !_runner.Process.HasExited;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public bool Connect()
        {
            return _runner.Start();
        }

        public void Disconnect()
        {
            _runner.Kill();
        }

        public void SendData(byte[] data)
        {
            _runner.InputWrite(data);
        }

        public void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {

        }
    }
}
