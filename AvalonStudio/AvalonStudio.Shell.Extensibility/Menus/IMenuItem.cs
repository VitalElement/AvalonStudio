using Avalonia.Media;
using System.Windows.Input;

namespace AvalonStudio.Menus
{
    public interface IMenuItem
    {
        string Label { get; }
        DrawingGroup Icon { get; }

        ICommand Command { get; }
    }
}
