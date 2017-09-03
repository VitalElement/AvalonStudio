using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using System;

namespace AvalonStudioLauncher
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = AppBuilder.Configure<App>();

            builder.UsePlatformDetect().UseSkia();

            builder.AfterSetup(b =>
            {
                Console.WriteLine("Launch main app");
            }).Start<SplashScreen>();
        }
    }

    class SplashScreen : Window
    {
        public SplashScreen()
        {
            Width = 800;
            Height = 450;
            HasSystemDecorations = false;
        }
    }
}
