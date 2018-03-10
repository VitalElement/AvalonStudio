using System;
using System.Composition;

namespace AvalonStudio.Debugging
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportDebuggerAttribute : ExportAttribute
    {
        public ExportDebuggerAttribute()
            : base(typeof(IDebugger))
        {
        }
    }
}
