using Perspex.Input;
using System;

namespace AvalonStudio
{
    class Program
    {        
        static void Main(string[] args)
        {
            var app = new App();
            var window = new MainWindow();
			var vm = Workspace.This;
            window.DataContext = vm;            

            window.Show();
            app.Run(window);
        }
    }
}
