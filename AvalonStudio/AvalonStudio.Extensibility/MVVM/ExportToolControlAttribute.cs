using System;
using System.Composition;

namespace AvalonStudio.MVVM
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportToolControlAttribute : ExportAttribute
    {
        public ExportToolControlAttribute() : base(typeof(ToolViewModel))
        {

        }
    }
}
