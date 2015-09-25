namespace AvalonStudio.Models
{
    public interface IConsole
    {
        void WriteLine(string data);

        void WriteLine();

        void Write(string data);

        void Write (char data);

        void Clear ();
    }
}
