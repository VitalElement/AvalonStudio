using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility.Controls;

namespace AvalonStudio.Updater
{
    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
