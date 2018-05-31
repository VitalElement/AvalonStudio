using Avalonia.Media;

namespace AvalonStudio.Menus
{
    public interface IMenuItemFactory
    {
        IMenuItem CreateCommandMenuItem(string commandName);
        IMenuItem CreateHeaderMenuItem(string label, DrawingGroup icon);
    }
}
