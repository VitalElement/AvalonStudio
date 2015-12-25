namespace AvalonStudio
{
    using System;

    public class Program
    {
        private static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }            

            var app = new App();
            var container = CompositionRoot.CreateContainer();            
            var window = new MainWindow();

            Workspace.Instance = container.GetExportedValue<Workspace>();

            window.DataContext = Workspace.Instance;
            window.Show();

            app.Run(window);

            Workspace.Instance.Cleanup();
        }
    }
}