using System.Threading.Tasks;
using NuGet.Common;

namespace AvalonStudio.Repositories
{
    public class ConsoleNuGetLogger : ILogger
    {
        public void LogDebug(string data) => $"DEBUG: {data}".Dump();

        public void LogVerbose(string data) => $"VERBOSE: {data}".Dump();

        public void LogInformation(string data) => $"INFORMATION: {data}".Dump();

        public void LogMinimal(string data) => $"MINIMAL: {data}".Dump();

        public void LogWarning(string data) => $"WARNING: {data}".Dump();

        public void LogError(string data) => $"ERROR: {data}".Dump();

        public void LogSummary(string data) => $"SUMMARY: {data}".Dump();

        public void LogInformationSummary(string data) => $"Infosummary: {data}".Dump();

        public void LogErrorSummary(string data) => $"LogErrorSummary: {data }".Dump();

        public void Log(LogLevel level, string data) => $"LOG: {data}".Dump();

        public Task LogAsync(LogLevel level, string data)
        {
            $"LOG: {data}".Dump();
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message)
        {
            $"LOG: {message.Message}".Dump();
        }

        public Task LogAsync(ILogMessage message)
        {
            $"LOG: {message.Message}".Dump();
            return Task.CompletedTask;
        }
    }
}