using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal static class MenuDefinitions
	{
		[MenuItem] public static MenuItemDefinition FileNewSolutionItem =
			new CommandMenuItemDefinition<NewSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

		[MenuItem] public static MenuItemDefinition FileOpenSolutionItem =
			new CommandMenuItemDefinition<OpenSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

		[MenuItem] public static MenuItemDefinition FileCloseSolutionItem =
			new CommandMenuItemDefinition<CloseSolutionCommandDefinition>(
				Extensibility.MainMenu.MenuDefinitions.FileCloseMenuGroup, 1);
	}
}