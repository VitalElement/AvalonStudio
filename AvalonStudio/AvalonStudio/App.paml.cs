using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
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

			Platform.Initialise();

			PackageSources.InitialisePackageSources();

			var container = CompositionRoot.CreateContainer();

			var builder = AppBuilder.Configure<App>().UsePlatformDetect().SetupWithoutStarting();

			var commandService = container.GetExportedValue<ICommandService>();
			IoC.RegisterConstant(commandService, typeof(ICommandService));

			var keyGestureService = container.GetExportedValue<ICommandKeyGestureService>();
			IoC.RegisterConstant(keyGestureService, typeof(ICommandKeyGestureService));

			var toolBarBuilder = container.GetExportedValue<IToolBarBuilder>();
			IoC.RegisterConstant(toolBarBuilder, typeof(IToolBarBuilder));

			ShellViewModel.Instance = container.GetExportedValue<ShellViewModel>();


			builder.Instance.RunWithMainWindow<MainWindow>();

			ShellViewModel.Instance.Cleanup();
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