using System.Threading.Tasks;
using AvalonStudio.Utils;
using NuGet.Common;

namespace AvalonStudio.Repositories
{
    public class ConsoleNuGetLogger : ILogger
    {
        private IConsole _console;

        public ConsoleNuGetLogger(IConsole console)
        {
            _console = console;
        }

        public void LogDebug(string data) => _console.WriteLine($"DEBUG: {data}");

        public void LogVerbose(string data) => _console.WriteLine($"VERBOSE: {data}");

        public void LogInformation(string data) => _console.WriteLine($"INFORMATION: {data}");

        public void LogMinimal(string data) => _console.WriteLine($"MINIMAL: {data}");

        public void LogWarning(string data) => _console.WriteLine($"WARNING: {data}");

        public void LogError(string data) => _console.WriteLine($"ERROR: {data}");

        public void LogSummary(string data) => _console.WriteLine($"SUMMARY: {data}");

        public void LogInformationSummary(string data) => _console.WriteLine($"Infosummary: {data}");

        public void LogErrorSummary(string data) => _console.WriteLine($"LogErrorSummary: {data }");

        public void Log(LogLevel level, string data) => _console.WriteLine($"LOG: {data}");

        public Task LogAsync(LogLevel level, string data)
        {
            _console.WriteLine($"LOG: {data}");
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message)
        {
            _console.WriteLine($"LOG: {message.Message}");
        }

        public Task LogAsync(ILogMessage message)
        {
            _console.WriteLine($"LOG: {message.Message}");
            return Task.CompletedTask;
        }
    }
}