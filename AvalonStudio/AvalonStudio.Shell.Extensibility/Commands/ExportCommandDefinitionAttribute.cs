using System;
using System.Composition;

namespace AvalonStudio.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ExportCommandDefinitionAttribute : ExportAttribute
    {
        public string Name { get; }

        public ExportCommandDefinitionAttribute(string name)
            : base(typeof(CommandDefinition))
        {
            Name = name;
        }
    }
}
