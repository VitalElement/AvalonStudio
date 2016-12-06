using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    public class RecentProjectViewModel : ViewModel
    {
        public RecentProjectViewModel(string name, string location)
        {
            this._name = name;
            this._location = location;

            ClickCommand = ReactiveCommand.Create();

            ClickCommand.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                var path = Path.Combine(location, name + ".asln");

                shell.OpenSolution(path);
            });
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set { this.RaiseAndSetIfChanged(ref _location, value); }
        }

        public ReactiveCommand<object> ClickCommand { get; }
    }
}