using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.MainMenu
{
    public class MenuDefinitions : IExtension
    {
        public static MenuDefinition<MainMenuBar> FileMenu = new MenuDefinition<MainMenuBar>(0, "_File");

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }
    }
}
