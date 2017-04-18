using AvalonStudio.Extensibility.MainMenu.Models;
using AvalonStudio.Extensibility.Menus;

namespace AvalonStudio.Extensibility.MainMenu
{
    public interface IMenuBuilder
    {
        void BuildMenuBar(MenuBarDefinition menuBarDefinition, MenuModel result);
    }
}