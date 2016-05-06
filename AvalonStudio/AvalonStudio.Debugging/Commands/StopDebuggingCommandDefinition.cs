namespace AvalonStudio.Debugging.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Debugging;
    using Extensibility;
    using Perspex.Controls;
    using Perspex.Input;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Utils;

    [CommandDefinition]
    class StopDebuggingCommandDefinition : CommandDefinition
    {
        public StopDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();

                manager.Stop();
            });
        }

        private ReactiveCommand<object> command;

        public override ICommand Command
        {
            get
            {
                return command;
            }
        }

        public override string Text
        {
            get
            {
                return "Stop";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Stops the current debug session.";
            }
        }
    }
}
