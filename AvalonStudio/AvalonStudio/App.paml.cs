namespace AvalonStudio
{
    using AvalonStudio.Extensibility;
    using AvalonStudio.Repositories;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Diagnostics;
    using Perspex.Logging.Serilog;
    using Perspex;
    using Serilog;
    using System;

    class App : Application
    {
        public App()
        {
            RegisterServices();
        }

        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            PackageSources.InitialisePackageSources();

            var container = CompositionRoot.CreateContainer();            

            var app = new App().UseWin32().UseDirect2D().LoadFromXaml();

            Workspace.Instance = container.GetExportedValue<Workspace>();
            WorkspaceViewModel.Instance = container.GetExportedValue<WorkspaceViewModel>();

            app.RunWithMainWindow<MainWindow>();

            WorkspaceViewModel.Instance.Cleanup();
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
