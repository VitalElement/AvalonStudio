using System;

namespace AvalonStudio.Extensibility.Commands
{
    public abstract class CommandListDefinition : CommandDefinitionBase
    {
        public override sealed string Text
        {
            get { return "[NotUsed]"; }
        }

        public override sealed string ToolTip
        {
            get { return "[NotUsed]"; }
        }

        public override sealed Uri IconSource
        {
            get { return null; }
        }

        public override sealed bool IsList
        {
            get { return true; }
        }
    }
}