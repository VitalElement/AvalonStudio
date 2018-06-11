using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvalonStudio.Shell.Controls
{
    public class MetroWindowTheme : Styles
    {
        public MetroWindowTheme()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}