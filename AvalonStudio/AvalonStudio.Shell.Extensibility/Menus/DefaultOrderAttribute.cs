using System;
using System.Composition;

namespace AvalonStudio.Menus
{
    [MetadataAttribute]
    public class DefaultOrderAttribute : Attribute
    {
        public int DefaultOrder { get; }

        public DefaultOrderAttribute(int order)
        {
            DefaultOrder = order;
        }
    }
}
