namespace AvalonStudio.Controls.Standard.Console
{
    using Avalonia.Controls;
    using Avalonia;
    using Avalonia.Markup.Xaml;
    public class ConsoleView : UserControl
    {
        public ConsoleView()
        {
            this.InitializeComponent();            
        }        

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
        }
    }
}
