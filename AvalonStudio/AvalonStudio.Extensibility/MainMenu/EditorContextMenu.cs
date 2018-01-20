using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Extensibility.MainMenu
{
    using AvalonStudio.Extensibility.Menus;
    using AvalonStudio.Extensibility.Plugin;

    public class TextEditorContextMenu : IExtension
    {
        static TextEditorContextMenu()
        {
            // Do Nothing
        }

        public static readonly MenuBarDefinition EditorContextMenu = new MenuBarDefinition();

        public static readonly MenuDefinition FileMenu = new MenuDefinition(EditorContextMenu, 0, "Editor");

        public static readonly MenuItemGroupDefinition FileNewOpenMenuGroup = new MenuItemGroupDefinition(FileMenu, 0);

        public static readonly MenuItemDefinition FileSaveAllMenuItem = new MenuItemDefinition(FileNewOpenMenuGroup, 1);

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}
