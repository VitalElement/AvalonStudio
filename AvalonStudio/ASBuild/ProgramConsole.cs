namespace ASBuild
{
    using AvalonStudio.Models;
    using System;

    class ProgramConsole : IConsole
    {
        public void Clear()
        {
            Console.Clear();
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
