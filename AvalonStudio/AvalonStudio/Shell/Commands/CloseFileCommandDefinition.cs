namespace AvalonStudio.Shell.Commands
{
    using AvalonStudio.Extensibility.Commands;

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
    }
}