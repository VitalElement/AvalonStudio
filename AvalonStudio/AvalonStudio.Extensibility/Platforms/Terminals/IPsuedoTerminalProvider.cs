namespace AvalonStudio.Platforms.Terminals
{
    public interface IPsuedoTerminalProvider
    {
        IPsuedoTerminal Create(int columns, int rows, string initialDirectory, string environment, string command, params string[] arguments);
    }
}
