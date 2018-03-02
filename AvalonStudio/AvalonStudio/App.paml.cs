using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using Serilog;
using System;
using System.Composition;

namespace AvalonStudio
{
    internal class App : Application
    {
        static void Print(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Print(ex.InnerException);
            }
        }

        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

                var builder = BuildAvaloniaApp().AfterSetup(async _ =>
                {
                    var extensionManager = new ExtensionManager();
                    var container = CompositionRoot.CreateContainer(extensionManager);

                    Platform.Initialise();
                    PackageSources.InitialisePackageSources();

                    var shellExportFactory = container.GetExport<ExportFactory<ShellViewModel>>();
                    ShellViewModel.Instance = shellExportFactory.CreateExport().Value;

                    await PackageManager.LoadAssetsAsync().ConfigureAwait(false);
                });

                InitializeLogging();

                builder.Start<MainWindow>();
            }
            catch (Exception e)
            {
                Print(e);
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI();

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