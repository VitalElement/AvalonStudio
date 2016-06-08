namespace AvalonStudio.Projects.CPlusPlus
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class TargetSettingsForm : UserControl
    {
        public TargetSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
