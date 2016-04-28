namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System;
    using System.ComponentModel.Composition;
    using Perspex.Input;
    using ReactiveUI;
    using Extensibility;
    [CommandDefinition]
    public class SaveFileCommandDefinition : CommandDefinition
    {
        public SaveFileCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                shell?.Save();
            });
        }

        public const string CommandName = "File.SaveFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Save"; }
        }

        public override string ToolTip
        {
            get { return "Save Tool Tip"; }
        }

        public override Uri IconSource
        {
            get { return new Uri(""); }
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
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture() { Key = Key.S, Modifiers = InputModifiers.Control } );
    }
}