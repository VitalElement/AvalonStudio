namespace AvalonStudio.Debugging.Commands
{
    using Avalonia.Input;
    using Avalonia.Media;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.Reactive.Linq;
    using System.Windows.Input;

    internal class StartDebuggingCommandDefinition : CommandDefinition
    {
        private ReactiveCommand<object> command;

        public override void Activation()
        {
            var manager = IoC.Get<IDebugManager2>();

            command = ReactiveCommand.Create(manager.CanStart);
            command.Subscribe(_ =>
            {
                if (!manager.SessionActive)
                {
                    manager.Start();
                }
                else
                {
                    manager.Continue();
                }
            });
        }

        public override ICommand Command
        {
            get { return command; }
        }

        public override string Text
        {
            get { return "Start Debugging"; }
        }

        public override string ToolTip
        {
            get { return "Starts a debug session."; }
        }

        public override DrawingGroup Icon => this.GetCommandIcon("Run");

        public override KeyGesture Gesture => KeyGesture.Parse("F5");
    }
}