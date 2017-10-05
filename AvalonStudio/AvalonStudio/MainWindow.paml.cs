using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;
using System;

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