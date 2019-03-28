using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        private TextReader _stdErrorReader;
        private TextReader _stdOutReader;

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
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,                    
                    CreateNoWindow = false,                    
                },
                EnableRaisingEvents = true
            };

            _process.StartInfo.Environment["TERM"] = "xterm-256color";

            stdErrorThread = new Thread(ErrorThread);
            stdOutputThread = new Thread(OutputThread);            

        }

        public bool Start()
        {
            if (!_started)
            {
                _started = _process.Start();

                _stdOutReader = TextReader.Synchronized(_process.StandardOutput);
                _stdErrorReader = TextReader.Synchronized(_process.StandardError);

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
            var data = new char[128];

            while (!IsRunning())
            {
                Thread.Sleep(100);
            }            

            while (true)
            {
                var bytesReceived = _stdErrorReader.Read(data, 0, data.Length);

                if (bytesReceived > 0)
                {
                    var result = Encoding.UTF8.GetBytes(data, 0, bytesReceived);

                    _stdErrorData.OnNext(result);
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

        }

        private void OutputThread()
        {
            var data = new char[128];

            while (!IsRunning())
            {
                Thread.Sleep(100);
            }            

            while (true)
            {
                var bytesReceived = _stdOutReader.Read(data, 0, data.Length);

                if (bytesReceived > 0)
                {
                    var result = Encoding.UTF8.GetBytes(data, 0, bytesReceived);

                    _stdOutData.OnNext(result);
                }
                else
                {
                    Thread.Sleep(10);
                }
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
