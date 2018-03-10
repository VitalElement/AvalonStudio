using AvalonStudio.Projects;
using Mono.Debugging.Client;

namespace AvalonStudio.Debugging
{
    public enum WatchFormat
    {
        Binary,
        Decimal,
        Hexadecimal,
        Octal,
        Natural
    }

    public interface IDebugger2 : IDebugger
    {
        DebuggerSession CreateSession(IProject project);

        DebuggerStartInfo GetDebuggerStartInfo(IProject project);

        DebuggerSessionOptions GetDebuggerSessionOptions(IProject project);
    }
}