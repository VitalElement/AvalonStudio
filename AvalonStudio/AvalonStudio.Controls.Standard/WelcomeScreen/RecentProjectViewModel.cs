using System;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class RecentProjectViewModel : ViewModel {
        public RecentProjectViewModel(string name, string location) {
            this.name = name;

            ClickCommand = ReactiveCommand.Create();

            ClickCommand.Subscribe(_ => {
                var shell = IoC.Get<IShell>();

                shell.OpenSolution(location);
            });
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        public ReactiveCommand<object> ClickCommand { get; }
    }
}