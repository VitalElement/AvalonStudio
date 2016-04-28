namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System;
    using System.ComponentModel.Composition;
    using Perspex.Input;

    [CommandDefinition]
    public class SaveFileCommandDefinition : CommandDefinition
    {
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

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture() { Key = Key.S, Modifiers = InputModifiers.Control } );
    }
}