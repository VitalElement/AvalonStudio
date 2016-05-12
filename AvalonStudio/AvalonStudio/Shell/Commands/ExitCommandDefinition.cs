namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using Avalonia.Input;
    using ReactiveUI;
    using System.ComponentModel.Composition;
    using System;
    using Extensibility;

    [CommandDefinition]
    public class ExitCommandDefinition : CommandDefinition
    {
        public ExitCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                Environment.Exit(0);
            });
        }

        public override string Text
        {
            get { return "Exit"; }
        }

        public override string ToolTip
        {
            get { return "Exit Tool Tip"; }
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