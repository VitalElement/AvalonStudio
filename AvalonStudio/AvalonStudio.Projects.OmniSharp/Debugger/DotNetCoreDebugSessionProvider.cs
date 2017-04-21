namespace AvalonStudio.Debugging.DotNetCore
{
    using AvalonStudio.CommandLineTools;
    using AvalonStudio.Extensibility;
    using AvalonStudio.GlobalSettings;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Projects.OmniSharp;
    using Mono.Debugging.Client;
    using Mono.Debugging.Win32;
    using System.IO;
    using AvalonStudio.Utils;
    using System;
    using System.Threading.Tasks;

    public class DotNetCoreDebugger : IDebugger2
    {
        public void Activation()
        {
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<DotNetCoreDebugger>(this);
        }

        private static string ResolveShimVersion()
        {
            var settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

            bool inHostSection = false;

            string result = string.Empty;

            var exitCode = PlatformSupport.ExecuteShellCommand(settings.DotNetPath, "--info", (s, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    if (inHostSection)
                    {
                        if (e.Data.Trim().StartsWith("Version"))
                        {
                            var parts = e.Data.Split(':');

                            if (parts.Length >= 2)
                            {
                                result = parts[1].Trim();
                            }
                        }
                    }
                    else
                    {
                        if (e.Data.Trim().StartsWith("Microsoft .NET Core Shared Framework Host"))
                        {
                            inHostSection = true;
                        }
                    }
                }
            }, (s, e) => { }, false, "", false);

            return result;
        }

        public DebuggerSession CreateSession(IProject project)
        {
            var settings = SettingsBase.GetSettings<DotNetToolchainSettings>();

            var coreAppVersion = ResolveShimVersion();

            var dbgShimPath = Path.Combine(Path.GetDirectoryName(settings.DotNetPath), "shared", "Microsoft.NETCore.App", coreAppVersion, "dbgshim" + Platform.DLLExtension);

            var result = new CoreClrDebuggerSession(System.IO.Path.GetInvalidPathChars(), dbgShimPath);

            result.CustomSymbolReaderFactory = new PdbSymbolReaderFactory();

            return result;
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
                Command = "dotnet" + Platforms.Platform.ExecutableExtension,
                Arguments = project.Executable,
                WorkingDirectory = System.IO.Path.GetDirectoryName(project.Executable),
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