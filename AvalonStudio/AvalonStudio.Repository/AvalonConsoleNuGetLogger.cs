using System.Threading.Tasks;
using AvalonStudio.Utils;
using NuGet.Common;

namespace AvalonStudio.Repositories
{
    public class AvalonConsoleNuGetLogger : ILogger
    {
        private IConsole _console;

        public AvalonConsoleNuGetLogger(IConsole console)
        {
            _console = console;
        }

        public void LogDebug(string data) => _console.WriteLine(data);

        public void LogVerbose(string data) => _console.WriteLine(data);

        public void LogInformation(string data) => _console.WriteLine(data);

        public void LogMinimal(string data) => _console.WriteLine(data);

        public void LogWarning(string data) => _console.WriteLine(data);

        public void LogError(string data) => _console.WriteLine(data);

        public void LogSummary(string data) => _console.WriteLine(data);

        public void LogInformationSummary(string data) => _console.WriteLine(data);

        public void LogErrorSummary(string data) => _console.WriteLine(data);

        public void Log(LogLevel level, string data) => _console.WriteLine(data);

        public Task LogAsync(LogLevel level, string data)
        {
            _console.WriteLine(data);
            return Task.CompletedTask;
        }

        public void Log(ILogMessage message) => _console.WriteLine(message.Message);

        public Task LogAsync(ILogMessage message)
        {
            _console.WriteLine(message.Message);

            return Task.CompletedTask;
        }
    }
}