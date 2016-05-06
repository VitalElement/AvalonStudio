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
    class StartDebuggingCommandDefinition : CommandDefinition
    {
        public StartDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();
                var shell = IoC.Get<IShell>();

                var project = shell.GetDefaultProject();

                manager.StartDebug(project);
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
                return "Start Debugging";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Starts a debug session.";
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StartDebuggingCommandDefinition>(new KeyGesture() { Key = Key.F5 });
    }
}
