using AvalonStudio.Extensibility.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
   static  class MenuDefinitions
    {
        [MenuItem]
        public static MenuItemDefinition FileNewSolutionItem = new CommandMenuItemDefinition<NewSolutionCommandDefinition>(Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

        [MenuItem]
        public static MenuItemDefinition FileOpenSolutionItem = new CommandMenuItemDefinition<OpenSolutionCommandDefinition>(Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);
    }
}
