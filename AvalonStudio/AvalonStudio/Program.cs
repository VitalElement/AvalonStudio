namespace AvalonStudio
{
    using Extensibility;
    using Platform;
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

            Platform.Platform.Initialise();

            PackageSources.InitialisePackageSources();

            var app = new App();
            var container = CompositionRoot.CreateContainer();            
            var window = new MainWindow();

            Workspace.Instance = container.GetExportedValue<Workspace>();            
            WorkspaceViewModel.Instance = container.GetExportedValue<WorkspaceViewModel>();

            window.DataContext = WorkspaceViewModel.Instance;
            window.Show();

            app.Run(window);

            WorkspaceViewModel.Instance.Cleanup();
        }
    }
}