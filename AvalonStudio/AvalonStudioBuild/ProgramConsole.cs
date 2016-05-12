﻿namespace AvalonStudio
{
    using AvalonStudio.Utils;
    using Platforms;
    using System;

    class ProgramConsole : IConsole
    {
        private bool canOverwrite = true;

        public ProgramConsole()
        {
            try
            {
                OverWrite(string.Empty);

                if(Platforms.Platform.PlatformIdentifier != PlatformID.Win32NT)
                {
                    canOverwrite = false;
                }
            }
            catch (Exception e)
            {
                canOverwrite = false;
            }
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void OverWrite(string data)
        {
            //if (canOverwrite)
            //{
            //    int currentLineCursor = Console.CursorTop;
            //    Console.SetCursorPosition(0, Console.CursorTop);
            //    Console.Write(new string(' ', Console.WindowWidth));
            //    Console.SetCursorPosition(0, currentLineCursor);
            //    Console.Write(data);
            //}
            //else
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
