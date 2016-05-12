namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using Avalonia.Input;
    using ReactiveUI;
    using System.ComponentModel.Composition;
    using System;
    using Extensibility;
    using Controls;
    [CommandDefinition]
    public class PackagesCommandDefinition : CommandDefinition
    {
        public PackagesCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                IShell shell = IoC.Get<IShell>();
                shell.ModalDialog = new PackageManagerDialogViewModel();
                shell.ModalDialog.ShowDialog();
            });
        }

        public override string Text
        {
            get { return "Packages"; }
        }

        public override string ToolTip
        {
            get { return "Packages Tool Tip"; }
        }

        ReactiveCommand<object> command;
        public override System.Windows.Input.ICommand Command
        {
            get
            {
                return command;
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture() { Key = Key.F4, Modifiers = InputModifiers.Alt });
    }
}