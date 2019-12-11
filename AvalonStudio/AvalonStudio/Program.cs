using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Dialogs;
using Avalonia.Logging.Serilog;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Packaging;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Terminals.Unix;
using AvalonStudio.Shell.Controls;
using Serilog;
using System;
using System.Threading.Tasks;

namespace AvalonStudio
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            UnixPsuedoTerminal.Trampoline(args);

#if !DEBUG
            try
            {
#endif
                if (args == null)
                {
                    throw new ArgumentNullException(nameof(args));
                }

            try
            {
                BuildAvaloniaApp().StartShellApp("AvalonStudio", AppMain, args);
            }
            catch(Exception e)
            {

            }
#if !DEBUG
            }
            catch (Exception e)
            {
                Print(e);
            }
            finally
#endif
            {
                (Application.Current.ApplicationLifetime as ClassicDesktopStyleApplicationLifetime)?.Shutdown(0);
            }
        }

        // Your application's entry point. Here you can initialize your MVVM framework, DI
        // container, etc.

        private static async void TestMethodAsync ()
        {
            await Task.Delay(500);

            throw new Exception("My Except");
        }

        private static async void AppMain(string[] args)
        {
            var studio = IoC.Get<IStudio>();

            InitializeLogging();

            Platform.Initialise();

            TestMethodAsync();

            Dispatcher.UIThread.Post(async () =>
            {
                await PackageManager.LoadAssetsAsync().ConfigureAwait(false);
            });
        }

        public static AppBuilder BuildAvaloniaApp()
        {
            var result = AppBuilder.Configure<App>();

            if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
            {
                result
                    .UseWin32()
                    .UseSkia();
            }
            else
            {
                result.UsePlatformDetect()
                    .UseManagedSystemDialogs<AppBuilder, MetroWindow>();
            }

            return result
                .With(new Win32PlatformOptions { AllowEglInitialization = true, UseDeferredRendering = true })
                .With(new MacOSPlatformOptions { ShowInDock = true })
                .With(new AvaloniaNativePlatformOptions { UseDeferredRendering = true, UseGpu = true })
                .With(new X11PlatformOptions { UseGpu = true, UseEGL = true });
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
    }
}
