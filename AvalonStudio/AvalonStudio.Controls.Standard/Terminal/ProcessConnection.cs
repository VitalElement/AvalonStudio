using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using AvalonStudio.Platforms;
using VtNetCore.Avalonia;
using static winpty.WinPty;

namespace AvalonStudio.Controls.Standard.Terminal
{
    class PtyConnection : IConnection
    {
        IntPtr handle = IntPtr.Zero;
        IntPtr err = IntPtr.Zero;
        IntPtr cfg = IntPtr.Zero;
        IntPtr spawnCfg = IntPtr.Zero;
        Stream stdin = null;
        Stream stdout = null;

        private bool _isConnected = false;

        public PtyConnection()
        {
            
        }

        public bool IsConnected => _isConnected;

        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public bool Connect()
        {
            try
            {
                cfg = winpty_config_new(WINPTY_FLAG_COLOR_ESCAPES, out err);
                winpty_config_set_initial_size(cfg, 80, 32);

                handle = winpty_open(cfg, out err);
                if (err != IntPtr.Zero)
                {
                    System.Console.WriteLine(winpty_error_code(err));
                    return false;
                }

                string exe = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                string args = "";
                string cwd = @"C:\";
                spawnCfg = winpty_spawn_config_new(WINPTY_SPAWN_FLAG_AUTO_SHUTDOWN, exe, args, cwd, null, out err);
                if (err != IntPtr.Zero)
                {
                    System.Console.WriteLine(winpty_error_code(err));
                    return false;
                }

                stdin = CreatePipe(winpty_conin_name(handle), PipeDirection.Out);
                stdout = CreatePipe(winpty_conout_name(handle), PipeDirection.In);

                if (!winpty_spawn(handle, spawnCfg, out IntPtr process, out IntPtr thread, out int procError, out err))
                {
                    System.Console.WriteLine(winpty_error_code(err));
                    return false;
                }

                new Thread(() =>
                {
                    var data = new byte[128];

                    while(true)
                    {

                        var bytesReceived = stdout.Read(data, 0, data.Length);

                        if(bytesReceived > 0)
                        {
                            var receviedData = new byte[bytesReceived];

                            Buffer.BlockCopy(data, 0, receviedData, 0, bytesReceived);

                            DataReceived?.Invoke(this, new DataReceivedEventArgs { Data = receviedData });
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                }).Start();

                _isConnected = true;

                return _isConnected;
            }
            finally
            {
                //stdin?.Dispose();
                //stdout?.Dispose();
                //winpty_config_free(cfg);
                //winpty_spawn_config_free(spawnCfg);
                //winpty_error_free(err);
                //winpty_free(handle);
            }
        }

        private Stream CreatePipe(string pipeName, PipeDirection direction)
        {
            string serverName = ".";
            if (pipeName.StartsWith("\\"))
            {
                int slash3 = pipeName.IndexOf('\\', 2);
                if (slash3 != -1)
                {
                    serverName = pipeName.Substring(2, slash3 - 2);
                }
                int slash4 = pipeName.IndexOf('\\', slash3 + 1);
                if (slash4 != -1)
                {
                    pipeName = pipeName.Substring(slash4 + 1);
                }
            }

            var pipe = new NamedPipeClientStream(serverName, pipeName, direction);
            pipe.Connect();
            return pipe;
        }

        public void Disconnect()
        {
            stdin?.Dispose();
            stdout?.Dispose();
            winpty_config_free(cfg);
            winpty_spawn_config_free(spawnCfg);
            winpty_error_free(err);
            winpty_free(handle);
        }

        public void SendData(byte[] data)
        {
            if(data.Length == 1 && data[0] ==10)
            {
                data[0] = 13;
            }

            stdin.Write(data, 0, data.Length);            
        }

        public void SetTerminalWindowSize(int columns, int rows, int width, int height)
        {
            if (cfg != IntPtr.Zero)
            {
                winpty_set_size(handle, columns, rows, out err);
            }
        }
    }
}
