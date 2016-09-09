using System.Collections.ObjectModel;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class WelcomeScreenViewModel : DocumentTabViewModel, IExtension {
        private ObservableCollection<RecentProjectViewModel> _recentProjects;

        public WelcomeScreenViewModel() {
            Title = "Welcome Screen";

            _recentProjects = new ObservableCollection<RecentProjectViewModel>();

            for (int i = 0; i < 10; i++) {
                _recentProjects.Add(new RecentProjectViewModel("name" + i, "null"));
            }
        }

        public ObservableCollection<RecentProjectViewModel> RecentProjects
        {
            get { return _recentProjects; }
            set { this.RaiseAndSetIfChanged(ref _recentProjects, value); }
        }

        public void Activation() {
            IoC.Get<IShell>().AddDocument(this);
        }

        public void BeforeActivation() {

        }
    }
}
