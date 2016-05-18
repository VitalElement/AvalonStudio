namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia;
    using Avalonia.Controls;

    public class ProjectConfigurationDialogView : UserControl
    {
        public ProjectConfigurationDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
