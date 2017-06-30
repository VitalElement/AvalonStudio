using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains.GCC;
using Mono.Debugging.Client;
using System;
using System.IO;
using AvalonStudio.Utils;
using System.Threading.Tasks;
using AvalonStudio.Packages;

namespace AvalonStudio.Debugging.GDB.JLink
{
    internal class JLinkDebugger : IDebugger2
    {
        public static string BaseDirectory
        {
            get
            {
                if (Platform.OSDescription == "Unix")
                {
                    return string.Empty;
                }
                else
                {
                    return Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Debuggers.JLink"), "content").ToPlatformPath();
                }
            }
        }

        public string BinDirectory => BaseDirectory;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<JLinkDebugger>(this);
        }

        public DebuggerSession CreateSession(IProject project)
        {
            if (project.ToolChain is GCCToolchain)
            {
                return new JLinkGdbSession(project, (project.ToolChain as GCCToolchain).GDBExecutable);
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
            return new JLinkSettingsFormViewModel(project);
        }

        public async Task InstallAsync(IConsole console)
        {
            await PackageManager.EnsurePackage("AvalonStudio.Debuggers.JLink", console);
        }
    }
}