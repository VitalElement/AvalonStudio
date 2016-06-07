namespace AvalonStudio.Projects.CPlusPlus
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

    public class ReferenceSettingsFormView : UserControl
    {
        public ReferenceSettingsFormView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
