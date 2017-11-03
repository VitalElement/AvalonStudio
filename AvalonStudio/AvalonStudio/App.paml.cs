using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using Serilog;
using System;

namespace AvalonStudio
{
    internal class App : Application
    {
        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var builder = AppBuilder.Configure<App>().UseReactiveUI().AvalonStudioPlatformDetect().AfterSetup(_ =>
            {
                Platform.Initialise();

                PackageSources.InitialisePackageSources();

                var container = CompositionRoot.CreateContainer();

                ShellViewModel.Instance = container.GetExport<ShellViewModel>();
            });

            InitializeLogging();

            builder.Start<MainWindow>();
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
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
            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                return builder.UseWin32().UseSkia();
            }
            else
            {
                return builder.UseGtk3().UseSkia();
            }
        }
    }
}