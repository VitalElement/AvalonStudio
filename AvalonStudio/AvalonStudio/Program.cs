using Perspex.Input;
using ReactiveUI;
using System;

namespace AvalonStudio
{
    class Program
    {        
        static void Main(string[] args)
        {            
            var app = new App();

            Workspace.This = new Workspace();
            
            ReactiveCommand.Create();
            var window = new MainWindow();
			var vm = Workspace.This;
            window.DataContext = vm;


            window.Show();
            app.Run(window);
        }
    }
}
