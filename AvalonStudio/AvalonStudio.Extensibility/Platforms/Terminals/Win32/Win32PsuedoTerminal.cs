using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using static winpty.WinPty;

namespace AvalonStudio.Platforms.Terminals.Win32
{
    public class Win32PsuedoTerminal : IPsuedoTerminal
    {
        private IntPtr _handle = IntPtr.Zero;
        private IntPtr _err = IntPtr.Zero;
        private IntPtr _cfg = IntPtr.Zero;
        private IntPtr _spawnCfg = IntPtr.Zero;
        private Stream _stdin = null;
        private Stream _stdout = null;
        private Process _process;

        public Win32PsuedoTerminal(Process process, IntPtr handle, IntPtr cfg, IntPtr spawnCfg, IntPtr err, Stream stdin, Stream stdout)
        {
            _process = process;

            _handle = handle;
            _stdin = stdin;
            _stdout = stdout;

            _cfg = cfg;
            _spawnCfg = spawnCfg;
            _err = err;
        }

        public void Dispose()
        {
            _stdin?.Dispose();
            _stdout?.Dispose();
            winpty_config_free(_cfg);
            winpty_spawn_config_free(_spawnCfg);
            winpty_error_free(_err);
            winpty_free(_handle);
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await _stdout.ReadAsync(buffer, offset, count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            if (buffer.Length == 1 && buffer[0] == 10)
            {
                buffer[0] = 13;
            }

            await _stdin.WriteAsync(buffer, offset, count);
        }

        public void SetSize(int columns, int rows)
        {
            if (_cfg != IntPtr.Zero && columns >= 1 && rows >= 1)
            {
                winpty_set_size(_handle, columns, rows, out _err);
            }
        }

        public Process Process => _process;
    }
}
