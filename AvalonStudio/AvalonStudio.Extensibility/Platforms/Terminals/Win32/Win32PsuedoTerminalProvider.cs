using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using System;
using System.IO;
using System.IO.Pipes;
using static winpty.WinPty;

namespace AvalonStudio.Platforms.Terminals.Win32
{
    public class Win32PsuedoTerminalProvider : IPsuedoTerminalProvider, IExtension
    {
        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            if (Platforms.Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                IoC.RegisterConstant<IPsuedoTerminalProvider>(this);
            }
        }

        public IPsuedoTerminal Create(int columns, int rows, string initialDirectory, string environment, string command, params string[] arguments)
        {
            var cfg = winpty_config_new(WINPTY_FLAG_COLOR_ESCAPES, out IntPtr err);
            winpty_config_set_initial_size(cfg, columns, rows);

            var handle = winpty_open(cfg, out err);

            if (err != IntPtr.Zero)
            {
                System.Console.WriteLine(winpty_error_code(err));
                return null;
            }

            string exe = command;
            string args = String.Join(" ", arguments);
            string cwd = initialDirectory;

            var spawnCfg = winpty_spawn_config_new(WINPTY_SPAWN_FLAG_AUTO_SHUTDOWN, exe, args, cwd, environment, out err);
            if (err != IntPtr.Zero)
            {
                System.Console.WriteLine(winpty_error_code(err));
                return null;
            }

            var stdin = CreatePipe(winpty_conin_name(handle), PipeDirection.Out);
            var stdout = CreatePipe(winpty_conout_name(handle), PipeDirection.In);

            if (!winpty_spawn(handle, spawnCfg, out IntPtr process, out IntPtr thread, out int procError, out err))
            {
                System.Console.WriteLine(winpty_error_code(err));
                return null;
            }

            return new Win32PsuedoTerminal(handle, cfg, spawnCfg, err, stdin, stdout);
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
    }
}
