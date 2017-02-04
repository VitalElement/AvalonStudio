using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
using AvalonStudio.Extensibility.Menus;
using System.Composition;
using System;
using AvalonStudio.Extensibility;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal static class MenuDefinitions
	{

        public class FileNewSolutionMenuItem : MenuItemDefinition
        {
            public FileNewSolutionMenuItem() : base(()=>IoC.Get<MenuItemGroupDefinition>(FileNewOpenMenuGroupDefinition.FileOpenMenuGroupContract),"New Solution", 0, ()=>IoC.Get<NewSolutionCommandDefinition>())
            {
                
            }
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