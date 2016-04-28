namespace AvalonStudio.Shell.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    [CommandDefinition]
    public class CopyCommandDefinition : CommandDefinition
    {
        public const string CommandName = "Edit.Copy";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Copy"; }
        }

        public override string ToolTip
        {
            get { return "Copy ToolTip"; }
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