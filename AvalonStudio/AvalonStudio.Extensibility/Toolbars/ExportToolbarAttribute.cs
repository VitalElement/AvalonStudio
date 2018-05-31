using System;
using System.Composition;

namespace AvalonStudio.Toolbars
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ExportToolbarAttribute : ExportAttribute
    {
        public string Name { get; }

        public ExportToolbarAttribute(string name)
            : base(typeof(Toolbar))
        {
            Name = name;
        }
    }
}
