using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Avalonia.Input;
using AvalonStudio.Extensibility;

namespace AvalonStudio.Shell.Commands
{
    class ShowQuickFinderCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> _command;
        private ShellViewModel _shell;

        public override void Activation()
        {
            _shell = IoC.Get<ShellViewModel>();
        }

        public ShowQuickFinderCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ => {
                
                _shell.ShowQuickCommander();
            });
        }

        public override KeyGesture Gesture => KeyGesture.Parse("CTRL+P");

        public override string Text => "Show Quick Commander";

        public override string ToolTip => Text;

        public override ICommand Command => _command;
    }
}
