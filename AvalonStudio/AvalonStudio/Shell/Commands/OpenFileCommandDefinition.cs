namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using Avalonia.Input;
    using System;
    using System.ComponentModel.Composition;

    [CommandDefinition]
    public class OpenFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.OpenFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Open File"; }
        }

        public override string ToolTip
        {
            get { return "Open File ToolTip"; }
        }

        public override Uri IconSource
        {
            get { return new Uri("pack://application:,,,/Gemini;component/Resources/Icons/Open.png"); }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<OpenFileCommandDefinition>(new KeyGesture() { Key = Key.O, Modifiers = InputModifiers.Control });
    }
}