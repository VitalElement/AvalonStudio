namespace AvalonStudio.Debuggers.GDB.Local
{
    using AvalonStudio.Debugging;
    using AvalonStudio.Debugging.GDB;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Toolchains.GCC;
    using Mono.Debugging.Client;
    using System;
    using System.IO;
    using AvalonStudio.Utils;
    using System.Threading.Tasks;

    public class LocalGdbDebugger : IDebugger2
    {
        public string BinDirectory => null;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<LocalGdbDebugger>(this);
        }

        public DebuggerSession CreateSession(IProject project)
        {
            if (project.ToolChain is GCCToolchain)
            {
                return new GdbSession((project.ToolChain as GCCToolchain).GDBExecutable, detectAsync: false);
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
                CloseExternalConsoleOnExit = true
            };

            return startInfo;
        }

        public object GetSettingsControl(IProject project)
        {
            return null;
        }

        public Task InstallAsync(IConsole console)
        {
            return Task.FromResult(0);
        }
    }
}