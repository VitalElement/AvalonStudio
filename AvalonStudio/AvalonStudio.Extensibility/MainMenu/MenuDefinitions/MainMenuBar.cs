using AvalonStudio.Extensibility.Menus;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.MainMenu
{
    class MainMenuBar : MenuBarDefinition
    {
        public override void Activation()
        {
            
        }

        public override void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(MenuBarDefinition), "MainMenu");
        }
    }
}
