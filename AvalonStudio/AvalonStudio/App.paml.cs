using Avalonia;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility;
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

                var builder = BuildAvaloniaApp().BuildShellApp().AfterSetup(async _ =>
                {
                    PackageSources.InitialisePackageSources();

                    await PackageManager.LoadAssetsAsync().ConfigureAwait(false);
                });

                InitializeLogging();

                builder.StartAvalonShellApp();
            }
            catch (Exception e)
            {
                Print(e);
            }
            finally
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