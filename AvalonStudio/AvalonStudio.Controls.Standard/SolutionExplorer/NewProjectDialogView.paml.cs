namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class NewProjectDialogView : UserControl
    {
        public NewProjectDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
