namespace AvalonStudio.Shell
{
    using AvalonStudio.Extensibility.Menus;
    using Commands;

    class MenuDefinitions
    {        
        [MenuItem]
        public static MenuItemDefinition FileNewMenuItem = new TextMenuItemDefinition(
            Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 0, "_New");

        [MenuGroup]
        public static MenuItemGroupDefinition FileNewCascadeGroup = new MenuItemGroupDefinition(
            FileNewMenuItem, 0);

        [MenuItem]
        public static MenuItemDefinition FileNewMenuItemList = new CommandMenuItemDefinition<NewFileCommandListDefinition>(
            FileNewCascadeGroup, 0);

        //[Export]
        //public static MenuItemDefinition FileOpenMenuItem = new CommandMenuItemDefinition<OpenFileCommandDefinition>(
        //    Extensibility.MainMenu.MenuDefinitions.FileNewOpenMenuGroup, 1);

        [MenuItem]
        public static MenuItemDefinition FileCloseMenuItem = new CommandMenuItemDefinition<CloseFileCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileCloseMenuGroup, 0);

        [MenuItem]
        public static MenuItemDefinition FileSaveMenuItem = new CommandMenuItemDefinition<SaveFileCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 0);

        [MenuItem]
        public static MenuItemDefinition FileSaveAsMenuItem = new CommandMenuItemDefinition<SaveFileAsCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileSaveMenuGroup, 1);

        [MenuItem]
        public static MenuItemDefinition FileExitMenuItem = new CommandMenuItemDefinition<ExitCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.FileExitOpenMenuGroup, 0);

        //[Export]
        //public static MenuItemDefinition WindowDocumentList = new CommandMenuItemDefinition<SwitchToDocumentCommandListDefinition>(
        //    Extensibility.MainMenu.MenuDefinitions.WindowDocumentListMenuGroup, 0);

        [MenuItem]
        public static MenuItemDefinition EditCopyMenuItem = new CommandMenuItemDefinition<CopyCommandDefinition>(
            Extensibility.MainMenu.MenuDefinitions.EditUndoRedoMenuGroup, 0);
    }
}
