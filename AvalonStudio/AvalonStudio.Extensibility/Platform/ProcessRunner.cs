using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Platforms
{
    public class ProcessRunner
    {
        private Process _process;
        private Thread stdErrorThread;
        private Thread stdOutputThread;
        private Subject<byte[]> _stdErrorData;
        private Subject<byte[]> _stdOutData;

        public IObservable<byte[]> StdErrorData => _stdErrorData;
        public IObservable<byte[]> StdOutData => _stdOutData;
        private bool _started;

        public ProcessRunner(string path, string arguments)
        {
            _stdErrorData = new Subject<byte[]>();
            _stdOutData = new Subject<byte[]>();

            _process = new Process
            {
                StartInfo = new ProcessStartInfo(path, arguments)
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    StandardErrorEncoding = Encoding.UTF8,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                },
                EnableRaisingEvents = true
            };

            stdErrorThread = new Thread(ErrorThread);
            stdOutputThread = new Thread(OutputThread);

        }

        public bool Start()
        {
            if (!_started)
            {
                _started = _process.Start();
                stdErrorThread.Start();
                stdOutputThread.Start();
            }

            return _started;
        }


        public void Kill()
        {
            _process.Kill();
        }

        public bool IsRunning()
        {
            if (_process == null)
                throw new ArgumentNullException("process");

            try
            {
                Process.GetProcessById(_process.Id);
            }
            catch (ArgumentException)
            {
                return false;
            }
            return true;
        }

        private void ErrorThread()
        {
            var data = new byte[1];

            while (!IsRunning())
            {
                Thread.Sleep(100);
            }

            while (true)
            {
                var bytesReceived = _process.StandardOutput.BaseStream.Read(data, 0, data.Length);

                _stdOutData.OnNext(data);
            }

        }

        private void OutputThread()
        {
            var data = new byte[1];

            while (!IsRunning())
            {
                Thread.Sleep(100);
            }

            while (true)
            {
                var bytesReceived = _process.StandardOutput.BaseStream.Read(data, 0, data.Length);

                _stdOutData.OnNext(data);
            }
        }

        public void InputWrite(byte[] data)
        {
            _process.StandardInput.BaseStream.Write(data, 0, data.Length);
            _process.StandardInput.BaseStream.Flush();
        }

        public Process Process => _process;

        public bool Started => _started;

    }
}
