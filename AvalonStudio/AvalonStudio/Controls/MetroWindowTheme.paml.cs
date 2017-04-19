using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvalonStudio.Controls
{
    public class MetroWindowTheme : Styles
    {
        public MetroWindowTheme()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}