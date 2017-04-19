namespace AvalonStudio.Extensibility
{
    using AvalonStudio.Extensibility.Menus;
    using AvalonStudio.Extensibility.Plugin;

    public class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {
            // Do Nothing
        }

        public static readonly MenuBarDefinition MainMenuBar = new MenuBarDefinition();

        public static readonly MenuDefinition FileMenu = new MenuDefinition(MainMenuBar, 0, "_File");

        public static readonly MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

        public static readonly MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

        public static readonly MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

        public static readonly MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

        public static readonly MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

        public static readonly MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

        public static readonly MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");

        public static readonly MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

        public static readonly MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

        public static readonly MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

        public static readonly MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

        public static readonly MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(WindowMenu, 10);

        public static readonly MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}