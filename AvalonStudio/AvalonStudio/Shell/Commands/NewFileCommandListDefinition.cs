using AvalonStudio.Extensibility.Commands;
using Avalonia.Input;
using System.ComponentModel.Composition;
using System;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
    [CommandDefinition]
    public class NewFileCommandDefinition : CommandDefinition
    {
        public override string Text
        {
            get
            {
                return "New File";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Creates a new file.";
            }
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
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<NewFileCommandDefinition>(new KeyGesture() { Key = Key.N, Modifiers = InputModifiers.Control });
    }
}