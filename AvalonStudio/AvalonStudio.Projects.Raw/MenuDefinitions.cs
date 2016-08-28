namespace AvalonStudio.Projects.Raw
{
    using AvalonStudio.Controls.Standard.SolutionExplorer.Commands;
    using AvalonStudio.Extensibility.Menus;

    internal static class MenuDefinitions
    {
        [MenuItem]
        public static MenuItemDefinition FileOpenFolderItem =
            new CommandMenuItemDefinition<OpenFolderCommandDefinition>(
                Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 2);
    }
}
