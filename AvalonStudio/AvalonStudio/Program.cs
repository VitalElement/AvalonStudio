namespace AvalonStudio
{
    using Controls;

    class Program
    {        
        static void Main(string[] args)
        {
            var app = new App();
            var window = new MainWindow();

            var editorModel = new EditorModel();

            Workspace.This = new Workspace(editorModel);
            window.DataContext = Workspace.This;

            window.Show();

            app.Run(window);

            editorModel.Shutdown();
        }
    }
}
