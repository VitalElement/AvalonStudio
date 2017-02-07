namespace AvalonStudio.Extensibility
{
    using AvalonStudio.Extensibility.Menus;
    using AvalonStudio.Extensibility.Plugin;

    public class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {

        }

        public static MenuBarDefinition MainMenuBar = new MenuBarDefinition();

        public static MenuDefinition FileMenu = new MenuDefinition(MainMenuBar, 0, "_File");

        public static MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

        public static MenuItemGroupDefinition FileCloseMenuGroup = new MenuItemGroupDefinition(FileMenu, 3);

        public static MenuItemGroupDefinition FileSaveMenuGroup = new MenuItemGroupDefinition(FileMenu, 6);

        public static MenuItemGroupDefinition FileExitOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 10);

        public static MenuDefinition EditMenu = new MenuDefinition(MainMenuBar, 1, "_Edit");

        public static MenuItemGroupDefinition EditUndoRedoMenuGroup = new MenuItemGroupDefinition(EditMenu, 0);

        public static MenuDefinition ViewMenu = new MenuDefinition(MainMenuBar, 2, "_View");
        
        public static MenuItemGroupDefinition ViewToolsMenuGroup = new MenuItemGroupDefinition(ViewMenu, 0);

        public static MenuItemGroupDefinition ViewPropertiesMenuGroup = new MenuItemGroupDefinition(ViewMenu, 100);

        public static MenuDefinition ToolsMenu = new MenuDefinition(MainMenuBar, 10, "_Tools");

        public static MenuDefinition WindowMenu = new MenuDefinition(MainMenuBar, 20, "_Window");

        public static MenuItemGroupDefinition WindowDocumentListMenuGroup = new MenuItemGroupDefinition(WindowMenu, 10);

        public static MenuDefinition HelpMenu = new MenuDefinition(MainMenuBar, 30, "_Help");

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {

        }

        

    }
}