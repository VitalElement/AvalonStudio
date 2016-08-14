using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Platforms;
using Serilog;
using SharpDX;

namespace AvalonStudio
{
	internal class App : Application
	{
		private static void Main(string[] args)
		{
			Configuration.EnableReleaseOnFinalizer = true;

			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				var message = (e.ExceptionObject as Exception)?.Message;

				if (message != null)
				{
					Console.WriteLine(message);
				}
			};

			if (args == null)
			{
				throw new ArgumentNullException(nameof(args));
			}

			var builder = AppBuilder.Configure<App>();

			if (args.Length >= 1 && args[0] == "--skia")
			{
				builder.UseSkia();
				
				if (Platform.PlatformIdentifier == PlatformID.Win32NT)
				{
					builder.UseWin32();
				}
				else
				{
					builder.UseGtk();
				}
			}
			else
			{
				builder.UsePlatformDetect();
			}

			builder.SetupWithoutStarting();

			var splash = new BootScreen();

			splash.Show();

			builder.Instance.Run(splash);
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
#if DEBUG
			SerilogLogger.Initialize(new LoggerConfiguration()
				.MinimumLevel.Warning()
				.WriteTo.Trace(outputTemplate: "{Area}: {Message}")
				.CreateLogger());
#endif
		}
	}
}