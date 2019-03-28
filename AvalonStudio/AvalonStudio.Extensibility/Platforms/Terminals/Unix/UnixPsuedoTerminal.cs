using AvalonStudio.Platforms.Terminals;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Platforms.Terminals.Unix
{
    public class UnixPsuedoTerminal : IPsuedoTerminal
    {
        private int _handle;
        private int _cfg;
        private Stream _stdin = null;
        private Stream _stdout = null;
        private Process _process;
        private bool _isDisposed = false;

        public UnixPsuedoTerminal(Process process, int handle, int cfg, Stream stdin, Stream stdout)
        {
            _process = process;

            _handle = handle;
            _stdin = stdin;
            _stdout = stdout;

            _cfg = cfg;
        }

        public static void Trampoline()
        {
            Native.setsid();
            Native.ioctl(0, Native.TIOCSCTTY, IntPtr.Zero);
            Native.execve("/bin/bash", new string[] { "/bin/bash", null }, new string[] { "TERM=xterm-256color", null });
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _stdin?.Dispose();
                _stdout?.Dispose();

                // TODO close file descriptors and terminate processes?
            }
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

            await Task.Run(() =>
            {
                var buf = Marshal.AllocHGlobal(count);
                Marshal.Copy(buffer, offset, buf, count);
                Native.write(_cfg, buf, count);

                Marshal.FreeHGlobal(buf);
            });

            //await _stdin.WriteAsync(System.Text.Encoding.UTF8.GetString(buffer).ToCharArray(), offset, count);

            //_stdin.WriteByte(buffer[0]);
            //_stdin.Flush();
            //_stdin.WriteByte(0);




            //await _stdin.FlushAsync();

            //await _stdin.FlushAsync();

        }

        public void SetSize(int columns, int rows)
        {
            Native.winsize size = new Native.winsize();
            int ret;
            size.ws_row = (ushort)(rows > 0 ? rows : 24);
            size.ws_col = (ushort)(columns > 0 ? columns : 80);
            ret = Native.ioctl(_cfg, Native.TIOCSWINSZ, ref size);

            var error = Marshal.GetLastWin32Error();
        }

        public struct winsize
        {
            public ushort ws_row;   /* rows, in characters */
            public ushort ws_col;   /* columns, in characters */
            public ushort ws_xpixel;    /* horizontal size, pixels */
            public ushort ws_ypixel;    /* vertical size, pixels */
        };

        public Process Process => _process;
    }
}
