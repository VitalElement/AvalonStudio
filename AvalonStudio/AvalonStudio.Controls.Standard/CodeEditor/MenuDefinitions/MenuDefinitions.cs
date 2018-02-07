using AvalonStudio.Controls.Standard.CodeEditor.Commands;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor.MenuDefinitions
{
    internal class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {
            // Do Nothing
        }

        public static readonly MenuItemDefinition NewSolutionMenuItem = new MenuItemDefinition<GotoDefinitionCommand>(Extensibility.MainMenu.TextEditorContextMenu.FileNewOpenMenuGroup, 0);

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}
