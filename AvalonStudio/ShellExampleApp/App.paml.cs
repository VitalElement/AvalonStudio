using Avalonia;
using Avalonia.Markup.Xaml;
using AvalonStudio.Shell;
using System;

namespace ShellExampleApp
{
    public class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            BuildAvaloniaApp().StartShellApp();

            Application.Current.Exit();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}