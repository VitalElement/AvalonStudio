using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.Shell;
using System;

namespace ShellExampleApp
{
    public class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            BuildAvaloniaApp().AfterSetup(builder=>
            {
                Dispatcher.UIThread.InvokeAsync(() => { ColorTheme.LoadTheme(ColorTheme.VisualStudioLight); });
            }).StartShellApp("ShellExampleApp");

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