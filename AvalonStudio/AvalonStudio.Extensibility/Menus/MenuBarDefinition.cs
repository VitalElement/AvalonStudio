using System;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Extensibility.Menus
{
    public abstract class MenuBarDefinition : IExtension
    {
        public abstract void Activation();

        public abstract void BeforeActivation();
    }
}