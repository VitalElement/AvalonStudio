using Avalonia;
using Avalonia.Markup.Xaml;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;

using System;

namespace AvalonStudio
{
    public class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            Console.Write("Initializing mainwindow...");
            InitializeComponent();
            Console.WriteLine("Done");

            DataContext = ShellViewModel.Instance;

            Console.WriteLine("Data Context set");

            //KeyBindings.AddRange(ShellViewModel.Instance.KeyBindings);

            //Console.WriteLine("KeyBindings set");

            var generalSettings = Settings.GetSettings<GeneralSettings>();

            Console.WriteLine($"{generalSettings == null}");
            ColorTheme.LoadTheme(generalSettings.Theme);

            Console.WriteLine("ColorTheme Loaded");

            //this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}