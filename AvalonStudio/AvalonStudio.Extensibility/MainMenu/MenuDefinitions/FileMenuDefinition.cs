using AvalonStudio.Extensibility.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.MainMenu.MenuDefinitions
{
    class FileMenuDefinition : MenuDefinition
    {
        public FileMenuDefinition() : base(()=>IoC.Get<MenuBarDefinition>("MainMenu"), 0, "_File")
        {
        }

        public override void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(MenuDefinition), "FileMenu");
        }
    }
}
