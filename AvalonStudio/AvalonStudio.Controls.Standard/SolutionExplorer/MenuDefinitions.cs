using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
using AvalonStudio.Extensibility.Menus;
using System.Composition;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal static class MenuDefinitions
	{
        public static MenuItemDefinition FileNewSolutionItem =
			new CommandMenuItemDefinition<NewSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

        
        public static MenuItemDefinition FileOpenSolutionItem =
			new CommandMenuItemDefinition<OpenSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

        public static MenuItemDefinition FileCloseSolutionItem =
			new CommandMenuItemDefinition<CloseSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileCloseMenuGroup, 1);
	}
}