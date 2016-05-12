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

    class App : Application
    {
        public App()
        {
            
            RegisterServices();
            this.UseWin32().UseDirect2D().LoadFromXaml();
        }

        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();
            var app = new App();

            var commandService = container.GetExportedValue<ICommandService>();
            IoC.RegisterConstant(commandService, typeof(ICommandService));

            var keyGestureService = container.GetExportedValue<ICommandKeyGestureService>();
            IoC.RegisterConstant(keyGestureService, typeof(ICommandKeyGestureService));

            var toolBarBuilder = container.GetExportedValue<IToolBarBuilder>();
            IoC.RegisterConstant(toolBarBuilder, typeof(IToolBarBuilder));
            
            ShellViewModel.Instance = container.GetExportedValue<ShellViewModel>();
            
            app.RunWithMainWindow<MainWindow>();

            ShellViewModel.Instance.Cleanup();
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
