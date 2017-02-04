using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal class MenuDefinitions : IExtension
	{
        public MenuItemDefinition NewSolutionMenuItem = new MenuItemDefinition<NewSolutionCommandDefinition>(Extensibility.MenuDefinitions.FileNewOpenMenuGroup, "New Solution", 0);

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        //     public static MenuItemDefinition FileNewSolutionItem =
        //new CommandMenuItemDefinition<NewSolutionCommandDefinition>(
        //	Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);


        //     public static MenuItemDefinition FileOpenSolutionItem =
        //new CommandMenuItemDefinition<OpenSolutionCommandDefinition>(
        //	Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0);

        //     public static MenuItemDefinition FileCloseSolutionItem =
        //new CommandMenuItemDefinition<CloseSolutionCommandDefinition>(
        //	Extensibility.MainMenu.MenuDefinitions.FileCloseMenuGroup, 1);
    }
}