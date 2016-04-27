namespace AvalonStudio.Extensibility.MainMenu
{
    using AvalonStudio.Extensibility.Menus;
    using Models;

    public interface IMenuBuilder
    {
        void BuildMenuBar(MenuBarDefinition menuBarDefinition, MenuModel result);
    }
}