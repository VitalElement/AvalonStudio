using AvalonStudio.Extensibility.Commands;
using ReactiveUI;

namespace AvalonStudio.Shell.Commands
{
    [CommandDefinition]
    public class SaveFileAsCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.SaveFileAs";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Save As"; }
        }

        public override string ToolTip
        {
            get { return "Save As Tool Tip"; }
        }

        ReactiveCommand<object> command;
        public override System.Windows.Input.ICommand Command
        {
            get
            {
                return command;
            }
        }
    }
}