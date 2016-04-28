using AvalonStudio.Extensibility.Commands;
using Perspex.Input;
using System.ComponentModel.Composition;
using System;

namespace AvalonStudio.Shell.Commands
{
    [CommandDefinition]
    public class NewFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.NewFile";

        public override string Name
        {
            get { return CommandName; }
        }

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

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<NewFileCommandDefinition>(new KeyGesture() { Key = Key.N, Modifiers = InputModifiers.Control });
    }
}