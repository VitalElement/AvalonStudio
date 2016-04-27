namespace AvalonStudio.Controls
{
    using Extensibility.Controls;
    using Perspex;
    using Perspex.Controls;

    public class ToolControl : ContentControl
    {
        public ToolControl()
        {            
            Styles.Add(new ControlTheme());
        }

        public static readonly PerspexProperty<string> TitleProprty =
            PerspexProperty.Register<ToolControl, string>(nameof(Title));

        public string Title
        {
            get { return GetValue(TitleProprty); }
            set { SetValue(TitleProprty, value); }
        }
    }
}
