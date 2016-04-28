namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using Perspex.Input;
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

        public const string CommandName = "File.Exit";

        public override string Name
        {
            get { return CommandName; }
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