namespace AvalonStudio.Extensibility.Shell
{
    public interface IStatusBar
    {
        bool SetText(string text);

        void ClearText();
    }
}
