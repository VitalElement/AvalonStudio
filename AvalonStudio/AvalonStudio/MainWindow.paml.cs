namespace AvalonStudio
{
    using Perspex.Controls;
    using Perspex.Input;
    using Perspex.Markup.Xaml;

    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            App.AttachDevTools(this);
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }        
    }
}
