﻿namespace VEBuild
{
    using AvalonStudio.Utils;
    using System;

    class ProgramConsole : IConsole
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void OverWrite(string data)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(data);
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
