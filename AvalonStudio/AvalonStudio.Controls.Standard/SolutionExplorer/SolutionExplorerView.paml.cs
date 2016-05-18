namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia;
    using Avalonia.Controls;

    public class SolutionExplorerView : UserControl
    {
        public SolutionExplorerView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
