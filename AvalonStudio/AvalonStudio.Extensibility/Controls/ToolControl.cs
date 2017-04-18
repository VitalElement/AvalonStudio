using Avalonia;
using Avalonia.Controls;

namespace AvalonStudio.Controls
{
    public class ToolControl : ContentControl
    {
        public static readonly AvaloniaProperty<string> TitleProprty =
            AvaloniaProperty.Register<ToolControl, string>(nameof(Title));

        public string Title
        {
            get { return GetValue(TitleProprty); }
            set { SetValue(TitleProprty, value); }
        }
    }
}