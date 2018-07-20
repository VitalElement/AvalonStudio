using Avalonia;
using Avalonia.Threading;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using AvalonStudio.Shell;
using Serilog;
using System;

namespace AvalonStudio
{
    internal class App : Application
    {
#if !DEBUG
        static void Print(Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);
            if (ex.InnerException != null)
            {
                Print(ex.InnerException);
            }
        }
#endif

        [STAThread]
        private static void Main(string[] args)
        {
#if !DEBUG
        try
            {
#endif
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            BuildAvaloniaApp().AfterSetup(async _ =>
            {
                InitializeLogging();

                Platform.Initialise();

                PackageSources.InitialisePackageSources();

                Dispatcher.UIThread.Post(async () =>
                   {
                    await PackageManager.LoadAssetsAsync().ConfigureAwait(false);
                });
            })
            .StartShellApp<AppBuilder, MainWindow>("AvalonStudio", null, () => new MainWindowViewModel());
#if !DEBUG
    }
            catch (Exception e)
            {
                Print(e);
            }
            finally
#endif
            {
                Application.Current.Exit();
            }
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect();

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
}