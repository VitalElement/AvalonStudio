using System;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;

namespace AvalonStudio
{
	public class BootScreen : SplashScreen
	{
		Timer time = new Timer();

		public BootScreen()
		{
			this.InitializeComponent();
			App.AttachDevTools(this);

			this.time.Interval = 3000;
			this.time.AutoReset = false;
			this.time.Elapsed += TimeOnElapsed;
			this.time.Start();
		}

		private void TimeOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
		{
			Startup();
		}

		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}

		public void Startup()
		{
			var progressBar = this.FindControl<ProgressBar>("StatusProgressBar");

			if (progressBar == null) throw new ApplicationException("Unable to locate progressbar");

			Platform.Initialise();
			Dispatcher.UIThread.InvokeAsync(() => progressBar.Value += 5);

			PackageSources.InitialisePackageSources();
			Dispatcher.UIThread.InvokeAsync(() => progressBar.Value += 5);

			var container = CompositionRoot.CreateContainer();
			Dispatcher.UIThread.InvokeAsync(() => progressBar.Value += 50);

			Dispatcher.UIThread.InvokeAsync(() =>
			{
				var commandService = container.GetExportedValue<ICommandService>();
				IoC.RegisterConstant(commandService, typeof(ICommandService));
				progressBar.Value += 10;

				var keyGestureService = container.GetExportedValue<ICommandKeyGestureService>();
				IoC.RegisterConstant(keyGestureService, typeof(ICommandKeyGestureService));
				progressBar.Value += 10;

				var toolBarBuilder = container.GetExportedValue<IToolBarBuilder>();
				IoC.RegisterConstant(toolBarBuilder, typeof(IToolBarBuilder));
				progressBar.Value += 10;
			});

			ShellViewModel.Instance = container.GetExportedValue<ShellViewModel>();
			Dispatcher.UIThread.InvokeAsync(() =>
			{
				var main = new MainWindow();

				this.Hide();

				main.Show();

				ShellViewModel.Instance.Cleanup();
			});
		}
	}
}
