using System;
using System.Composition;

namespace AvalonStudio.Toolchains
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class)]
    public class ExportToolchainAttribute : ExportAttribute
    {
        public ExportToolchainAttribute()
            : base(typeof(IToolchain))
        {
        }
    }
}
