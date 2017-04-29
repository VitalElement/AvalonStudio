using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Markup.Xaml;
using AvalonStudio.Platforms;
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

            var builder = AppBuilder.Configure<App>();

            if (args.Length >= 1 && args[0] == "--skia")
            {
                builder.UseSkia();

                if (Platform.OSDescription == "Windows")
                {
                    builder.UseWin32();
                }
                else
                {
                    builder.UseGtk3();
                }
            }
            else
            {
                builder.UsePlatformDetect();
            }

            builder.Start<BootScreen>();            
        }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }

        private static void InitializeLogging()
        {
        }
    }
}