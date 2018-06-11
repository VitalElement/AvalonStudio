using Avalonia;
using Avalonia.Markup.Xaml;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;

namespace AvalonStudio
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