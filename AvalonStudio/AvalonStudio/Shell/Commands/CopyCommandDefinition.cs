namespace AvalonStudio.Shell.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    [CommandDefinition]
    public class CopyCommandDefinition : CommandDefinition
    {
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