using System;
using System.Globalization;

namespace AvalonStudio.Extensibility
{
    public class ChangingOutput : IDisposable
    {
        private static readonly int ResultLen = 10;

        private static readonly string ClearString = "\r" + new string(' ', Console.WindowWidth - 1) + "\r";
        private static readonly int MaxDescLength = Console.WindowWidth - ResultLen - 4; // [1 - 1 - 2], or bufferExtent - space - brackets
        private readonly string _desc;

        public ChangingOutput(string format, params object[] args)
            : this(string.Format(format, args))
        {
        }

        public ChangingOutput(string test)
        {
            _desc = test.Truncate(MaxDescLength).PadRight(MaxDescLength);
            Print();
        }

        public void Clear()
        {
            Console.Write(ClearString);
        }

        public void Print()
        {
            Clear();
            Console.Write(_desc);
        }

        public void PrintResult(bool passed)
        {
            Print();
            Console.Write(" [");

            var c = Console.ForegroundColor;
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write((passed ? "Okay" : "FAILED").Center(ResultLen));
            Console.ForegroundColor = c;

            Console.Write("]");
        }

        public void PrintProgress(double progress)
        {
            var percentAmount = (int)(progress * 100) / ResultLen;

            var str = "";
            if (percentAmount > 0)
            {
                str = (percentAmount == 1 ? "" : new string('=', percentAmount - 1)) + ">";
            }

            Print();
            Console.Write(" [{0}]", str.PadRight(ResultLen));
        }

        public void PrintNumber(int num)
        {
            Print();
            Console.Write(" [{0}]", num.ToString(CultureInfo.InvariantCulture).Truncate(ResultLen).Center(ResultLen));
        }

        public void FinishLine()
        {
            Console.Write("\r");
            Console.WriteLine();
        }

        public void Dispose()
        {
            FinishLine();
        }
    }
}