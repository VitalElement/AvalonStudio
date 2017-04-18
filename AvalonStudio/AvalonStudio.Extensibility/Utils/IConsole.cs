namespace AvalonStudio.Utils
{
    public interface IConsole
    {
        void WriteLine(string data);

        void WriteLine();

        void OverWrite(string data);

        void Write(string data);

        void Write(char data);

        void Clear();
    }
}