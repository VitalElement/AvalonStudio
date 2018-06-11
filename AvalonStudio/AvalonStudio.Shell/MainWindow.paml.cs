using Avalonia;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Shell.Controls;

namespace AvalonStudio.Shell
{
    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = ShellViewModel.Instance;

            KeyBindings.AddRange(ShellViewModel.Instance.KeyBindings);

            var generalSettings = Settings.GetSettings<GeneralSettings>();
            ColorTheme.LoadTheme(generalSettings.Theme);

            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}