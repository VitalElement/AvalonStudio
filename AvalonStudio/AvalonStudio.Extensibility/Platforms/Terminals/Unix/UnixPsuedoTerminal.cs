using AvalonStudio.Platforms.Terminals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Platforms.Terminals.Unix
{
    internal class LinuxNativeMethods
    {
        //int open(const char *pathname, int flags);
        [DllImport("libc.so")]
        internal static extern int open(string name, int flags);

        //ssize_t read(int fd, void *buf, size_t count);
        [DllImport("libc.so")]
        internal static extern int read(int fd, byte[] buffer, int length);

        //ssize_t write(int fd, const void *buf, size_t count); 
        [DllImport("libc.so")]
        internal static extern int write(int fd, byte[] buffer, int length);

        //int grantpt(int fd);
        [DllImport("libc.so")]
        internal static extern int grantpt(int fd);

        //int unlockpt(int fd);
        [DllImport("libc.so")]
        internal static extern int unlockpt(int fd);

        //i later marshall the pointer to a string
        //char *ptsname(int fd);
        [DllImport("libc.so")]
        internal static extern IntPtr ptsname(int fd);
    }
    class UnixPsuedoTerminal : IPsuedoTerminal
    {
        public Process Process => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void SetSize(int columns, int rows)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
