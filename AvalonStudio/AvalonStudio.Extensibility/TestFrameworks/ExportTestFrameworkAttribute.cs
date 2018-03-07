using System.Composition;

namespace AvalonStudio.TestFrameworks
{
    public class ExportTestFrameworkAttribute : ExportAttribute
    {
        public ExportTestFrameworkAttribute()
            : base(typeof(ITestFramework))
        {
        }
    }
}
