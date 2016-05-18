namespace AvalonStudio.Debugging
{
    using Avalonia;
    using Avalonia.Controls;

    public class VariableControlView : UserControl
    {
        public VariableControlView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
