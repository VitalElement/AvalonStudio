using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Languages
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Property)]
    public class FileExtensionsAttribute : Attribute
    {
        public IEnumerable<string> Extensions { get; }

        public FileExtensionsAttribute(params string[] extensions)
        {
            Extensions = extensions;
        }
    }
}
