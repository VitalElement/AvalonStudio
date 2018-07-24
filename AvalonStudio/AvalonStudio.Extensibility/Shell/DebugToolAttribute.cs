using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.Shell
{
    public enum Perspective
    {
        Normal,
        Debugging
    }

    public class PerspectiveAttribute : Attribute
    {
        Perspective Perspective { get; }

        public PerspectiveAttribute(Perspective perspective)
        {
            Perspective = perspective;
        }
    }
}
