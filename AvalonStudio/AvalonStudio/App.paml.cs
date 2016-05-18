namespace AvalonStudio
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Repositories;
    using Extensibility.Commands;
    using Extensibility.ToolBars;
    using MVVM;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Diagnostics;
    using Avalonia.Logging.Serilog;
    using Serilog;
    using System;
    using Avalonia.Markup.Xaml;

    class App : Application
    {
        public App()
        {
        }

        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();
            
            var builder = AppBuilder.Configure<App>().UseWin32().UseDirect2D1().SetupWithoutStarting();

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
