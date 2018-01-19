namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    using Avalonia.Media.Imaging;
    using AvalonStudio.Controls.Standard.SolutionExplorer;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Platforms;
    using AvalonStudio.Shell;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Threading.Tasks;

    public class WelcomeScreenViewModel : DocumentTabViewModel, IExtension
    {
        private ObservableCollection<RecentProjectViewModel> _recentProjects;
        private ObservableCollection<NewsFeedViewModel> _newsFeed;
        private ObservableCollection<VideoFeedViewModel> _videoFeed;
        ISolutionExplorer _solutionExplorer;
        private CompositeDisposable _disposables;

        public WelcomeScreenViewModel()
        {
            Title = "Start Page";

            _recentProjects = new ObservableCollection<RecentProjectViewModel>();
            _newsFeed = new ObservableCollection<NewsFeedViewModel>();
            _videoFeed = new ObservableCollection<VideoFeedViewModel>();

            NewSolution = ReactiveCommand.Create(() => _solutionExplorer?.NewSolution());
            OpenSolution = ReactiveCommand.Create(() => _solutionExplorer?.OpenSolution());

            LoadRecentProjects();
        }

        ~WelcomeScreenViewModel()
        {

        }

        public void Activation()
        {
            var shell = IoC.Get<IShell>();
            shell.AddDocument(this, false);

            _disposables = new CompositeDisposable
            {
                Observable.FromEventPattern<SolutionChangedEventArgs>(shell, nameof(shell.SolutionChanged)).Subscribe(o => ShellOnSolutionChanged(o.Sender, o.EventArgs))
            };
            //shell.SolutionChanged += ShellOnSolutionChanged;

            //LoadNewsFeed().GetAwaiter().GetResult();
            //LoadVideoFeed().GetAwaiter().GetResult();
            _solutionExplorer = IoC.Get<ISolutionExplorer>();
        }

        public override void Close()
        {
            base.Close();

            _disposables.Dispose();
        }

        public void BeforeActivation()
        {
        }

        private void LoadRecentProjects()
        {
            _recentProjects.Clear();

            var recentProjects = RecentProjectsCollection.RecentProjects;

            if (recentProjects == null)
            {
                recentProjects = new List<RecentProject>();
            }

            for (int i = 0; i < 8; i++)
            {
                if (i < recentProjects.Count)
                {
                    _recentProjects.Add(new RecentProjectViewModel(recentProjects[i].Name, recentProjects[i].Path));
                }
                else
                {
                    break;
                }
            }
        }

        //private async Task LoadNewsFeed()
        //{
        //    // RSS Releated
        //    //var rssurl = @"http://sxp.microsoft.com/feeds/2.0/devblogs";
        //    //var reader = XmlReader.Create(rssurl);
        //    /*var feed = await LoadFeed(reader);
        //    reader.Close();*/

        //    /*if (feed == null)
        //    {
        //        return;
        //    }

        //    foreach (var syndicationItem in feed.Items)
        //    {
        //        var content = syndicationItem.Summary.Text;

        //        int maxCharCount = 150;

        //        if (content.Length >= maxCharCount)
        //        {
        //            content = content.StripHTML().Truncate(maxCharCount, "...");
        //        }

        //        var link = syndicationItem.Links.LastOrDefault();
        //        var url = "";

        //        if (link != null)
        //        {
        //            url = link.Uri.AbsoluteUri;
        //        }

        //        _newsFeed.Add(new NewsFeedViewModel(url, content, syndicationItem.Categories.Count > 0 ? syndicationItem.Categories[0].Label : "null", syndicationItem.Authors[0].Name, syndicationItem.Title.Text));
        //    }*/
        //}

        //private async Task LoadVideoFeed()
        //{
        //    var rssurl = @"https://www.youtube.com/feeds/videos.xml?channel_id=UCOWs5Rx9ot7p10mqYyzjyUA";
        //    //var reader = XmlReader.Create(rssurl);
        //    /*var feed = await LoadFeed(reader);

        //    reader.Close();

        //    if (feed == null)
        //    {
        //        return;
        //    }

        //    foreach (var syndicationItem in feed.Items)
        //    {
        //        var youtubeID = syndicationItem.Id.Replace("yt:video:", "");
        //        var url = "https://www.youtube.com/watch?v=" + youtubeID;

        //        var image = await SaveThumbnail(youtubeID);

        //        _videoFeed.Add(new VideoFeedViewModel(url, syndicationItem.Title.Text, image));
        //    }*/
        //}

        /*private async Task<SyndicationFeed> LoadFeed(XmlReader reader)
        {
            return await Task.Factory.StartNew<SyndicationFeed>(() =>
            {
                return SyndicationFeed.Load(reader);
            });
        }*/

        private async Task<IBitmap> SaveThumbnail(string youtubeID)
        {
            var savePath = Path.Combine(Platform.CacheDirectory, "videoFeed", youtubeID + ".png");

            if (!Directory.Exists(Path.Combine(Platform.CacheDirectory, "videoFeed")))
            {
                Directory.CreateDirectory(Path.Combine(Platform.CacheDirectory, "videoFeed"));
            }

            if (File.Exists(savePath))
            {
                return new Bitmap(savePath);
            }

            var thumbnail = "https://i4.ytimg.com/vi/" + youtubeID + "/hqdefault.jpg";

            /*using (WebClient client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(thumbnail), savePath);
            }*/

            return new Bitmap(savePath);
        }

        private void ShellOnSolutionChanged(object sender, SolutionChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                var newProject = new RecentProject
                {
                    Name = e.NewValue.Name,
                    Path = e.NewValue.Location
                };

                if (RecentProjectsCollection.RecentProjects == null)
                {
                    RecentProjectsCollection.RecentProjects = new List<RecentProject>();
                }

                if (RecentProjectsCollection.RecentProjects.Contains(newProject))
                {
                    RecentProjectsCollection.RecentProjects.Remove(newProject);
                }

                RecentProjectsCollection.RecentProjects.Insert(0, newProject);

                RecentProjectsCollection.Save();

                LoadRecentProjects();
            }
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

        public ObservableCollection<VideoFeedViewModel> VideoFeed
        {
            get { return _videoFeed; }
            set { this.RaiseAndSetIfChanged(ref _videoFeed, value); }
        }

        public ReactiveCommand NewSolution { get; }
        public ReactiveCommand OpenSolution { get; }
    }
}