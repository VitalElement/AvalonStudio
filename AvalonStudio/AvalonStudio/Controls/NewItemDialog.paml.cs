namespace AvalonStudio.Controls
{
    using Avalonia;
    using Avalonia.Controls;

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
