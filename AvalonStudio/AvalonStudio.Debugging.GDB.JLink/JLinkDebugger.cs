using System;
using System.Collections.Generic;
using System.Text;
using AvalonStudio.Projects;
using Mono.Debugging.Client;
using AvalonStudio.Extensibility;
using AvalonStudio.Toolchains.GCC;
using System.IO;
using AvalonStudio.Platforms;

namespace AvalonStudio.Debugging.GDB.JLink
{
    class JLinkDebugger : IDebugger2
    {
        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<JLinkDebugger>(this);
        }

        public DebuggerSession CreateSession(IProject project)
        {
            if(project.ToolChain is GCCToolchain)
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
    }
}
