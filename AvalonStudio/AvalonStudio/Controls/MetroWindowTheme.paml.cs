namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Styling;

    public class MetroWindowTheme : Styles
    {
        public MetroWindowTheme()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }        
    }
}
