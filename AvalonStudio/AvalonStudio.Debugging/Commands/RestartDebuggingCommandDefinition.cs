using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class RestartDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStop);

            command.Subscribe(_ =>
            {
                manager.Restart();
            });
        }

        public override DrawingGroup Icon => this.GetCommandIcon("Restart");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Restart Debugging"; }
        }

        public override string ToolTip
        {
            get { return "Restarts a debug session."; }
        }
    }
}