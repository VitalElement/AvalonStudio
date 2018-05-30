using System;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DefaultKeyGesturesAttribute : Attribute
    {
        public IEnumerable<string> DefaultKeyGestures { get; }

        public DefaultKeyGesturesAttribute(params string[] keyGestures)
        {
            DefaultKeyGestures = keyGestures;
        }
    }
}
