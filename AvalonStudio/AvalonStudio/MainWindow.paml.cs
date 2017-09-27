using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;

namespace AvalonStudio
{
    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            IoC.RegisterConstant<Window>(this);

            InitializeComponent();

            DataContext = ShellViewModel.Instance;

            KeyBindings.AddRange(IoC.Get<ShellViewModel>().KeyBindings);

            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}