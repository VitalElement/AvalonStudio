using AvalonStudio.Controls.Standard.AboutScreen.Commands;
using AvalonStudio.Extensibility.Menus;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
    internal class MenuDefinitions : IExtension
    {
        static MenuDefinitions()
        {
            // Do Nothing
        }

        public static readonly MenuItemGroupDefinition HelpAboutGroup =
           new MenuItemGroupDefinition(Extensibility.MenuDefinitions.HelpMenu, 300);

        public static readonly MenuItemDefinition HelpAboutItem =
            new MenuItemDefinition<AboutScreenCommandDefinition>(HelpAboutGroup, 300);

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}