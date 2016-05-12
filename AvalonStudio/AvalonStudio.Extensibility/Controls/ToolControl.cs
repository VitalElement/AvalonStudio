namespace AvalonStudio.Controls
{
    using Extensibility.Controls;
    using Avalonia;
    using Avalonia.Controls;

    public class ToolControl : ContentControl
    {
        public ToolControl()
        {            
            Styles.Add(new ControlTheme());            
        }

        public static readonly AvaloniaProperty<string> TitleProprty =
            AvaloniaProperty.Register<ToolControl, string>(nameof(Title));

        public string Title
        {
            get { return GetValue(TitleProprty); }
            set { SetValue(TitleProprty, value); }
        }

        
    }
}
