using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.Remote
{
    class RemoteGdbSession : GdbSession
    {
        private IConsole console;
        private IProject _project;

        public RemoteGdbSession(IProject project, string gdbExecutable) : base(gdbExecutable)
        {
            this._project = project;
            console = IoC.Get<IConsole>();

            TargetReady += RemoteGdbSession_TargetReady;
        }

        private void RemoteGdbSession_TargetReady(object sender, Mono.Debugging.Client.TargetEventArgs e)
        {
            var settings = _project.GetDebuggerSettings<RemoteGdbSettings>();
            bool result = RunCommand("-target-select", "extended-remote", $":{settings.Port}").Status == CommandStatus.Done;

            if (result)
            {
                var commands = settings.GDBInitCommands?.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

                foreach(var command in commands)
                {
                    var commandParts = command.Split(' ');
                    var args = command.Remove(0, commandParts[0].Length);

                    var arguments = args.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    RunCommand(commandParts[0], arguments);
                }

                console.WriteLine("[JLink] - Connected.");
            }
        }
    }
}
