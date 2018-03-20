using System;
using System.Composition;

namespace AvalonStudio.Shell
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportEditorProviderAttribute : ExportAttribute
    {
        public ExportEditorProviderAttribute()
            : base(typeof(IEditorProvider))
        {
        }
    }
}
