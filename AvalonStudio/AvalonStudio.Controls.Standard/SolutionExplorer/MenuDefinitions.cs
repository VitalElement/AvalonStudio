using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal class MenuDefinitions : IExtension
	{
        static MenuDefinitions()
        {

        }

        public static MenuItemDefinition NewSolutionMenuItem = new MenuItemDefinition<NewSolutionCommandDefinition>(Extensibility.MenuDefinitions.FileNewOpenMenuGroup, "New Solution", 0);
        
        public static MenuItemDefinition FileOpenSolutionItem = new MenuItemDefinition<OpenSolutionCommandDefinition>(Extensibility.MenuDefinitions.FileNewOpenMenuGroup, "Open Solution", 0);

        public static MenuItemDefinition FileCloseSolutionItem = new MenuItemDefinition<CloseSolutionCommandDefinition>(Extensibility.MenuDefinitions.FileCloseMenuGroup, "Close Solution", 1);

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {

        }
    }
}