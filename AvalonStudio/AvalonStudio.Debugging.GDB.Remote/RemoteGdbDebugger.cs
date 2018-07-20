using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Composition;
using System.IO;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.Remote
{
    [Shared]
    [ExportDebugger]
    internal class RemoteGdbDebugger : IDebugger2
    {
        public string BinDirectory => null;

        public DebuggerSession CreateSession(IProject project)
        {
            if (project.ToolChain is GCCToolchain)
            {
                return new RemoteGdbSession(project, (project.ToolChain as GCCToolchain).GDBExecutable);
            }

            throw new Exception("No toolchain");
        }

        public DebuggerSessionOptions GetDebuggerSessionOptions(IProject project)
        {
            var evaluationOptions = EvaluationOptions.DefaultOptions.Clone();

            evaluationOptions.EllipsizeStrings = false;
            evaluationOptions.GroupPrivateMembers = false;
            evaluationOptions.EvaluationTimeout = 1000;

            return new DebuggerSessionOptions() { EvaluationOptions = evaluationOptions };
        }

        public DebuggerStartInfo GetDebuggerStartInfo(IProject project)
        {
            var startInfo = new DebuggerStartInfo()
            {
                Command = Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath(),
                Arguments = "",
                WorkingDirectory = System.IO.Path.GetDirectoryName(Path.Combine(project.CurrentDirectory, project.Executable)),
                UseExternalConsole = false,
                RequiresManualStart = true,
                CloseExternalConsoleOnExit = true
            };

            return startInfo;
        }

        public object GetSettingsControl(IProject project)
        {
            return new RemoteGdbSettingsFormViewModel(project);
        }

        public Task<bool> InstallAsync(IConsole console, IProject project)
        {
            return Task.FromResult(true);
        }
    }
}
