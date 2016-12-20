namespace AvalonStudio.Debugging.ClrDbg
{
    using AvalonStudio.Debugging.GDB;
    using AvalonStudio.Extensibility.Threading;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Toolchains;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class NetCoreDebugAdaptor : GDBDebugAdaptor
    {
        public override string Name => "DotNetCore Debugger";

        public static string BaseDirectory => Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp\\.debugger\\").ToPlatformPath();

        public override async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
        {
            DebugMode = true;

            await base.StartAsync(toolchain, console, project, Path.Combine(BaseDirectory, "clrdbg" + Platform.ExecutableExtension), false, Path.GetDirectoryName(project.Executable));

            await SafelyExecuteCommand(async () => await new EnablePrettyPrintingCommand().Execute(this));

            await SafelyExecuteCommand(async () => await new ExecArgumentsCommand(Path.GetFileName(project.Executable)).Execute(this));

            await SafelyExecuteCommand(async () => await new SetBreakPointCommand("Program.cs", 7).Execute(this));

            return true;
        }

        public override async Task RunAsync()
        {
            if (CurrentState != DebuggerState.Running)
            {
                await new RunCommand("dotnet").Execute(this);
            }
        }
    }
}
