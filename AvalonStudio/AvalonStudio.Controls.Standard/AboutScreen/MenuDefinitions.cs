using AvalonStudio.Controls.Standard.AboutScreen.Commands;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
	internal static class MenuDefinitions
	{
		 public static MenuItemGroupDefinition HelpAboutGroup =
			new MenuItemGroupDefinition(Extensibility.MainMenu.MenuDefinitions.HelpMenu, 300);

		public static MenuItemDefinition HelpAboutItem =
			new CommandMenuItemDefinition<AboutScreenCommandDefinition>(HelpAboutGroup, 300);
	}
}