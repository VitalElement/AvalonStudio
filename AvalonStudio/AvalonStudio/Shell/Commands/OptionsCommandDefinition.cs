using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class OptionsCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand _command;

        public OptionsCommandDefinition()
        {
            _command = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();
                shell.AddDocument(IoC.Get<SettingsDialogViewModel>());
            });
        }

        public override string Text => "Options";

        public override string ToolTip => "IDE Settings";
        public override ICommand Command => _command;
    }
}