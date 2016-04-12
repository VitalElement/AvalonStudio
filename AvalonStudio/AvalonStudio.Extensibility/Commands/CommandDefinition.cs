using System;

namespace AvalonStudio.Extensibility.Commands
{
    public abstract class CommandDefinition : CommandDefinitionBase
    {
        public override Uri IconSource
        {
            get { return null; }
        }

        public sealed override bool IsList
        {
            get { return false; }
        }
    }
}