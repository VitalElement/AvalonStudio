using Avalonia.Controls;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class NewItemDialogView : UserControl
    {
        public NewItemDialogView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
