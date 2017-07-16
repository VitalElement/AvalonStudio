using AvalonStudio.Utils;
using System;
using System.Text;

namespace AvalonStudio.Toolchains.Standard
{
    class StringBuilderConsole : IConsole
    {
        private StringBuilder _builder = new StringBuilder();

        public string GetOutput()
        {
            return _builder.ToString().Trim();
        }

        public void Clear()
        {
            _builder.Clear();
        }

        public void OverWrite(string data)
        {
            throw new NotSupportedException();
        }

        public void Write(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                _builder.Append(data);
            }
        }

        public void Write(char data)
        {
            _builder.Append(data);
        }

        public void WriteLine(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                _builder.AppendLine(data);
            }
        }

        public void WriteLine()
        {
            _builder.AppendLine();
        }
    }
}
