using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ServiceModel.Syndication;
using System.Xml;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class WelcomeScreenViewModel : DocumentTabViewModel, IExtension {
        private ObservableCollection<RecentProjectViewModel> _recentProjects;
        private ObservableCollection<NewsFeedViewModel> _newsFeed;

        public WelcomeScreenViewModel() {
            Title = "Welcome Screen";

            _recentProjects = new ObservableCollection<RecentProjectViewModel>();
            _newsFeed = new ObservableCollection<NewsFeedViewModel>();

            var recentProjects = RecentProjectsCollection.RecentProjects;

            if(recentProjects == null)
                recentProjects = new List<RecentProject>();

            for (int i = 0; i < 5; i++) {
                if (i < recentProjects.Count) {
                    _recentProjects.Add(new RecentProjectViewModel(recentProjects[i].Name, recentProjects[i].Path));
                }
            }


            // RSS Releated
            var RSSURL = @"http://sxp.microsoft.com/feeds/2.0/devblogs";
            var reader = XmlReader.Create(RSSURL);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            if (feed == null)
                return;

            foreach (var syndicationItem in feed.Items) {
                _newsFeed.Add(new NewsFeedViewModel(syndicationItem.Summary.Text, syndicationItem.Categories.Count > 0 ? syndicationItem.Categories[0].Label : "null", syndicationItem.Authors[0].Name, syndicationItem.Title.Text));
            }
        }

        private void ShellOnSolutionChanged(object sender, SolutionChangedEventArgs solutionChangedEventArgs) {
            var newProject = new RecentProject {
                Name = solutionChangedEventArgs.NewValue.Name,
                Path = solutionChangedEventArgs.NewValue.CurrentDirectory
            };

            if(RecentProjectsCollection.RecentProjects == null)
                RecentProjectsCollection.RecentProjects = new List<RecentProject>();


            if (RecentProjectsCollection.RecentProjects.Contains(newProject)) {
                RecentProjectsCollection.Save();
                return;
            }

            RecentProjectsCollection.RecentProjects.Add(newProject);

            RecentProjectsCollection.Save();
        }

        public void Activation() {
            var shell = IoC.Get<IShell>();
            shell.AddDocument(this);
            shell.SolutionChanged += ShellOnSolutionChanged;
        }

        public void BeforeActivation() {

        }

        public ObservableCollection<RecentProjectViewModel> RecentProjects
        {
            get { return _recentProjects; }
            set { this.RaiseAndSetIfChanged(ref _recentProjects, value); }
        }

        public ObservableCollection<NewsFeedViewModel> NewsFeed
        {
            get { return _newsFeed; }
            set { this.RaiseAndSetIfChanged(ref _newsFeed, value); }
        }
    }
}
