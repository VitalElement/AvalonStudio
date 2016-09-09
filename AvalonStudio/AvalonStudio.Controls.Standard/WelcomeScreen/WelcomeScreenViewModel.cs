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
                if (i < recentProjects.Count) {
                    _recentProjects.Add(new RecentProjectViewModel(recentProjects[i].Name, recentProjects[i].Path));
                }
            }
        }

        private void ShellOnSolutionChanged(object sender, SolutionChangedEventArgs solutionChangedEventArgs) {
            var newProject = new RecentProject {
                Name = solutionChangedEventArgs.NewValue.Name,
                Path = solutionChangedEventArgs.NewValue.CurrentDirectory
            };


            RecentProjectsCollection.RecentProjects.Add(newProject);

            RecentProjectsCollection.Save();
        }

        public ObservableCollection<RecentProjectViewModel> RecentProjects
        {
            get { return _recentProjects; }
            set { this.RaiseAndSetIfChanged(ref _recentProjects, value); }
        }

        public void Activation() {
            var shell = IoC.Get<IShell>();
            shell.AddDocument(this);
            shell.SolutionChanged += ShellOnSolutionChanged;
        }

        public void BeforeActivation() {

        }
    }
}
