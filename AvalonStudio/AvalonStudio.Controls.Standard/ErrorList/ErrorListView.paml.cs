namespace AvalonStudio.Controls.Standard.ErrorList
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    public class ErrorListView : UserControl
    {
        public ErrorListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {

            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
