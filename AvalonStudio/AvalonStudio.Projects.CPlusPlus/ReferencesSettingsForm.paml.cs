namespace AvalonStudio.Projects.CPlusPlus
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class ReferencesSettingsForm : TabItem
    {
        public ReferencesSettingsForm()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
