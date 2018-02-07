using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using Serilog;
using System;

namespace AvalonStudio
{
    internal class App : Application
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var builder = AppBuilder.Configure<App>().UseReactiveUI().AvalonStudioPlatformDetect().AfterSetup(async _ =>
            {
                Platform.Initialise();

                PackageSources.InitialisePackageSources();

                var container = CompositionRoot.CreateContainer();

                ShellViewModel.Instance = container.GetExport<ShellViewModel>();

                await PackageManager.LoadAssetsAsync();
            });

            InitializeLogging();

            builder.Start<MainWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

            // DataTemplates.Add(new ViewLocatorDataTemplate());
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }
    }

    public static class AppBuilderExtensions
    {
        public static AppBuilder AvalonStudioPlatformDetect(this AppBuilder builder)
        {
            switch (Platform.PlatformIdentifier)
            {
                case Platforms.PlatformID.Win32NT:
                    return builder.UseWin32().UseSkia();

                case Platforms.PlatformID.Unix:
                    return builder.UseGtk3().UseSkia();

                default:
                    return builder.UsePlatformDetect();
            }
        }
    }
}