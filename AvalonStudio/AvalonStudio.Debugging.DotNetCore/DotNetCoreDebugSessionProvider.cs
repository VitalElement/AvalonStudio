namespace AvalonStudio.Debugging.DotNetCore
{
    using System;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Projects;
    using Mono.Debugging.Client;
    using Mono.Debugging.Win32;

    public class DotNetCoreDebugger : IDebugger2
    {
        public void Activation()
        {

        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<DotNetCoreDebugger>(this);
        }

        public DebuggerSession CreateSession(IProject project)
        {
            var result = new CoreClrDebuggerSession(System.IO.Path.GetInvalidPathChars(), "C:\\Program Files\\dotnet\\shared\\Microsoft.NETCore.App\\1.1.1\\dbgshim.dll");

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

        public void ProvisionSettings(IProject project)
        {
            
        }
    }
}
