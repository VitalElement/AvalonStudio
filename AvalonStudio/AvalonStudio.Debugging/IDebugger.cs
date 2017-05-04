namespace AvalonStudio.Debugging
{
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Projects;
    using Mono.Debugging.Client;

    public enum WatchFormat
    {
        Binary,
        Decimal,
        Hexadecimal,
        Octal,
        Natural
    }

    public interface IDebugger2 : IDebugger, IExtension, IInstallable
    {
        DebuggerSession CreateSession(IProject project);

        DebuggerStartInfo GetDebuggerStartInfo(IProject project);

        DebuggerSessionOptions GetDebuggerSessionOptions(IProject project);        
    }
}