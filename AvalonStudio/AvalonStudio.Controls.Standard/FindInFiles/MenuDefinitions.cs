using AvalonStudio.Controls.Standard.FindInFiles.Commands;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {
            // Do Nothing
        }

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }

        public static readonly MenuItemGroupDefinition EditFindGroup =
           new MenuItemGroupDefinition(Extensibility.MenuDefinitions.EditMenu, 300);

        public static readonly MenuItemDefinition FindInFilesItem =
            new MenuItemDefinition<FindInFilesCommandDefinition>(EditFindGroup, 300);
    }
}
