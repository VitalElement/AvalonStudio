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
            public FileNewSolutionMenuItem() : base(()=>IoC.Get<MenuItemGroupDefinition>("FileNewOpenGroup"), 0)
            {
            }

            public override string Text => "New Solution";

            public override Uri IconSource => null;

            public override KeyGesture KeyGesture => new KeyGesture() { Key = Key.N, Modifiers = InputModifiers.Control };

            public override CommandDefinitionBase CommandDefinition => null;
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