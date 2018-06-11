using System;
using System.Composition;

namespace AvalonStudio.Menus
{
    [MetadataAttribute]
    public class DefaultGroupAttribute : Attribute
    {
        public string DefaultGroup { get; }

        public DefaultGroupAttribute(string name)
        {
            DefaultGroup = name;
        }
    }
}
