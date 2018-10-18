using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
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
                var studio = IoC.Get<IStudio>();

                studio.OpenSolutionAsync(_location);
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