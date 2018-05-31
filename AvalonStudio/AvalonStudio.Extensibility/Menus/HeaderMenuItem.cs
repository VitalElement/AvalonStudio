using Avalonia.Media;
using System.Windows.Input;

namespace AvalonStudio.Menus
{
    internal class HeaderMenuItem : IMenuItem
    {
        public string Label { get; }
        public DrawingGroup Icon { get; }

        public ICommand Command => null;

        public HeaderMenuItem(string label, DrawingGroup icon)
        {
            Label = label;
            Icon = icon;
        }
    }
}
