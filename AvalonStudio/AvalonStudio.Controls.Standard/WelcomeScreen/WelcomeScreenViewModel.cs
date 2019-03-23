using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Microsoft.SyndicationFeed;
using AvalonStudio.Controls.Standard.SolutionExplorer;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.SyndicationFeed.Rss;
using System.Linq;
using System.Net;
using System.Reactive;

namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    [Export(typeof(WelcomeScreenViewModel))]
    [Export(typeof(IExtension))]
    [Shared]
    public class WelcomeScreenViewModel : DocumentTabViewModel, IActivatableExtension
    {
        private ISolutionExplorer _solutionExplorer;

        private ObservableCollection<RecentProjectViewModel> _recentProjects;
        private ObservableCollection<NewsFeedViewModel> _newsFeed;
        private ObservableCollection<VideoFeedViewModel> _videoFeed;
        private CompositeDisposable _disposables;

        [ImportingConstructor]
        public WelcomeScreenViewModel(ISolutionExplorer solutionExplorer)
        {
            Title = "Start Page";

            _solutionExplorer = solutionExplorer;

            _recentProjects = new ObservableCollection<RecentProjectViewModel>();
            _newsFeed = new ObservableCollection<NewsFeedViewModel>();
            _videoFeed = new ObservableCollection<VideoFeedViewModel>();

            NewSolution = ReactiveCommand.Create(_solutionExplorer.NewSolution);
            OpenSolution = ReactiveCommand.Create(_solutionExplorer.OpenSolution);

            LoadRecentProjects();
        }

        ~WelcomeScreenViewModel()
        {

        }

        public void Activation()
        {
            var shell = IoC.Get<IShell>();
            var studio = IoC.Get<IStudio>();

            shell.AddOrSelectDocument(this);

            _disposables = new CompositeDisposable
            {
                Observable.FromEventPattern<SolutionChangedEventArgs>(studio, nameof(studio.SolutionChanged)).Subscribe(o => ShellOnSolutionChanged(o.Sender, o.EventArgs))
            };
            //shell.SolutionChanged += ShellOnSolutionChanged;

            LoadNewsFeed();
            //LoadVideoFeed();
            _solutionExplorer = IoC.Get<ISolutionExplorer>();
        }

        public override bool OnClose()
        {
            bool result = base.OnClose();

            _disposables.Dispose();

            return result;
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

        private async void LoadNewsFeed()
        {
            // RSS Releated
            var rssurl = @"http://go.microsoft.com/fwlink/?linkid=84795&clcid=409";

            try
            {
                using (var reader = XmlReader.Create(rssurl))
                {
                    var feedReader = new RssFeedReader(reader);

                    while (await feedReader.Read())
                    {
                        switch (feedReader.ElementType)
                        {
                            case SyndicationElementType.Item:
                                var syndicationItem = await feedReader.ReadItem();

                                var content = syndicationItem.Description;

                                int maxCharCount = 150;

                                if (content.Length >= maxCharCount)
                                {
                                    content = content.StripHTML().Truncate(maxCharCount, "...");
                                }

                                var link = syndicationItem.Links.LastOrDefault();
                                var url = "";

                                if (link != null)
                                {
                                    url = link.Uri.AbsoluteUri;
                                }

                                _newsFeed.Add(new NewsFeedViewModel(url, content, syndicationItem.Categories.FirstOrDefault()?.Name, syndicationItem.Contributors.FirstOrDefault()?.Name, syndicationItem.Title));
                                break;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private async void LoadVideoFeed()
        {
            var rssurl = @"https://www.youtube.com/feeds/videos.xml?channel_id=UCOWs5Rx9ot7p10mqYyzjyUA";
            using (var reader = XmlReader.Create(rssurl))
            {
                var feedReader = new RssFeedReader(reader);

                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        case SyndicationElementType.Item:
                            var item = await feedReader.ReadItem();

                            var youtubeID = item.Id.Replace("yt:video:", "");
                            var url = "https://www.youtube.com/watch?v=" + youtubeID;

                            var image = await SaveThumbnail(youtubeID);

                            _videoFeed.Add(new VideoFeedViewModel(url, item.Title, image));
                            break;
                    }
                }

            }
        }

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

            using (var client = new WebClient())
            {
                await client.DownloadFileTaskAsync(new Uri(thumbnail), savePath);
            }

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

        public ReactiveCommand<Unit, Unit> NewSolution { get; }
        public ReactiveCommand<Unit, Unit> OpenSolution { get; }
    }
}