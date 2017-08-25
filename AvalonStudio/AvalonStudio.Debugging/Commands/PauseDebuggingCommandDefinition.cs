using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Styling;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Windows.Input;

namespace AvalonStudio.Debugging.Commands
{
    internal class PauseDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanPause);
            command.Subscribe(_ =>
            {
                manager.Pause();
            });
        }

        public override DrawingGroup Icon => this.GetCommandIcon("PauseDebugger");

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Pause"; }
        }

        public override string ToolTip
        {
            get { return "Pauses the current debug session."; }
        }
    }
}