using System;
using System.Threading.Tasks;

namespace AvalonStudio.Platforms.Terminals
{
    public interface IPsuedoTerminal : IDisposable
    {
        void SetSize(int columns, int rows);

        Task WriteAsync(byte[] buffer, int offset, int count);

        Task<int> ReadAsync(byte[] buffer, int offset, int count);
    }
}
