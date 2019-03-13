using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System;
using System.Composition;

namespace AvalonStudio
{
    [Export(typeof(IConsole))]
    [Shared]
    internal class ProgramConsole : IConsole
    {
        private bool canOverwrite = true;

        public ProgramConsole()
        {
            try
            {
                OverWrite(string.Empty);

                if (Platform.PlatformIdentifier != Platforms.PlatformID.Win32NT)
                {
                    canOverwrite = false;
                }
            }
            catch (Exception)
            {
                canOverwrite = false;
            }
        }

        public void Clear()
        {
            //Console.Clear();
        }

        public void OverWrite(string data)
        {
            if (canOverwrite)
            {
                Console.Write("\r" + data);
            }
            else
            {
                WriteLine(data);
            }
        }

        public void Write(char data)
        {
            Console.Write(data);
        }

        public void Write(string data)
        {
            Console.Write(data);
        }

        public void WriteLine()
        {
            Console.WriteLine();
        }

        public void WriteLine(string data)
        {
            if (data != null)
            {
                Console.WriteLine(data);
            }
        }
    }
}