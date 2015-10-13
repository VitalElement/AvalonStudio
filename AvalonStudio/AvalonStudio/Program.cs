using AvalonStudio.Controls;
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
            EditorModel editorModel = new EditorModel();
            Workspace.This = new Workspace(editorModel);
            
            ReactiveCommand.Create();
            var window = new MainWindow();
			var vm = Workspace.This;
            window.DataContext = vm;


            window.Show();
            app.Run(window);
        }
    }
}
