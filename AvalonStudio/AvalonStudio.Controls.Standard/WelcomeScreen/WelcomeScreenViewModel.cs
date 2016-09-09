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

            var recentProjects = RecentProjectsCollection.RecentProjects;

            for (int i = 0; i < 5; i++) {
                if (recentProjects.Count >= 5) {
                    _recentProjects.Add(new RecentProjectViewModel(recentProjects[i].Name, recentProjects[i].Path));
                }
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
