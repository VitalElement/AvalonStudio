using System;

namespace AvalonStudio
{
    class Program
    {        
        static void Main(string[] args)
        {
            var app = new App();
            var window = new MainWindow();
            var vm = new Workspace();
            window.DataContext = vm;

            window.Show();
            app.Run(window);
        }
    }
}
