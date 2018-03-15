using System;
using System.Composition;

namespace AvalonStudio.TestFrameworks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportTestFrameworkAttribute : ExportAttribute
    {
        public ExportTestFrameworkAttribute()
            : base(typeof(ITestFramework))
        {
        }
    }
}
