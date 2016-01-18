namespace AvalonStudio
{
    using Extensibility.Platform;
    using Repositories;
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {            
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var app = new App();
            var container = CompositionRoot.CreateContainer();            
            var window = new MainWindow();

            WorkspaceViewModel.Instance = container.GetExportedValue<WorkspaceViewModel>();

            window.DataContext = WorkspaceViewModel.Instance;
            window.Show();

            app.Run(window);

            WorkspaceViewModel.Instance.Cleanup();
        }
    }
}