namespace AvalonStudio.Shell.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    [CommandDefinition]
    public class CloseFileCommandDefinition : CommandDefinition
    {
        public const string CommandName = "File.CloseFile";

        public override string Name
        {
            get { return CommandName; }
        }        

        public override string Text
        {
            get { return "Close"; }
        }

        public override string ToolTip
        {
            get { return "Close ToolTip"; }
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