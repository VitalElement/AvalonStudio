using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Theme;
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

            this.AttachDevTools();

            LoadTheme(ColorTheme.VisualStudioLight);
        }

        public void LoadTheme(ColorTheme theme)
        {
            Resources["ThemeBackgroundBrush"] = theme.Background;
            Resources["ThemeControlDarkBrush"] = theme.ControlDark;
            Resources["ThemeControlMidBrush"] = theme.ControlMid;
            Resources["ThemeForegroundBrush"] = theme.Foreground;
            Resources["ThemeBorderDarkBrush"] = theme.BorderDark;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}