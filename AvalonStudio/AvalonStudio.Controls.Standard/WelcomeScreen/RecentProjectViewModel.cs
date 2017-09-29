using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.IO;

namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    public class RecentProjectViewModel : ViewModel
    {
        public RecentProjectViewModel(string name, string location)
        {
            _name = name;
            _location = location;

            ClickCommand = ReactiveCommand.Create(() =>
            {
                var shell = IoC.Get<IShell>();

                shell.OpenSolutionAsync(_location);
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

        public ReactiveCommand ClickCommand { get; }
    }
}