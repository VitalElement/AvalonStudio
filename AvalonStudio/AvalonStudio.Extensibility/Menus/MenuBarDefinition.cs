using System;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility;

namespace AvalonStudio.Extensibility.Menus
{
    public class MenuBarDefinition
    {
        public MenuBarDefinition()
        {
            IoC.RegisterConstant(this);
        }
    }
}