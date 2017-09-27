using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class StopDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(()=>manager.Stop(), manager.CanStop);
        }

        public override DrawingGroup Icon => this.GetCommandIcon("Stop");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Stop"; }
        }

        public override string ToolTip
        {
            get { return "Stops the current debug session."; }
        }
    }
}