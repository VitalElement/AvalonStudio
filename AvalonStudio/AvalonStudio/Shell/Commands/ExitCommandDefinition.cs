namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using Perspex.Input;
    using ReactiveUI;
    using System.ComponentModel.Composition;

    [CommandDefinition]
    public class ExitCommandDefinition : CommandDefinition
    {
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