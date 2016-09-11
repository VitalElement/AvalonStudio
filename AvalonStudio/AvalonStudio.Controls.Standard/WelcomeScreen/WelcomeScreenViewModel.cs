﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using Avalonia.Media.Imaging;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class WelcomeScreenViewModel : DocumentTabViewModel, IExtension {
        private ObservableCollection<RecentProjectViewModel> _recentProjects;
        private ObservableCollection<NewsFeedViewModel> _newsFeed;
        private ObservableCollection<VideoFeedViewModel> _videoFeed;

        public WelcomeScreenViewModel() {
            Title = "Welcome Screen";

            _recentProjects = new ObservableCollection<RecentProjectViewModel>();
            _newsFeed = new ObservableCollection<NewsFeedViewModel>();
            _videoFeed = new ObservableCollection<VideoFeedViewModel>();

            var recentProjects = RecentProjectsCollection.RecentProjects;

            if (recentProjects == null)
                recentProjects = new List<RecentProject>();

            for (int i = 0; i < 5; i++) {
                if (i < recentProjects.Count) {
                    _recentProjects.Add(new RecentProjectViewModel(recentProjects[i].Name, recentProjects[i].Path));
                }
            }

            // RSS Releated
            var rssurl = @"http://sxp.microsoft.com/feeds/2.0/devblogs";
            var reader = XmlReader.Create(rssurl);
            var feed = SyndicationFeed.Load(reader);
            reader.Close();

            if (feed == null)
                return;

            foreach (var syndicationItem in feed.Items) {
                var content = syndicationItem.Summary.Text;

                int maxCharCount = 150;

                if (content.Length >= maxCharCount) {
                    content = content.Remove(maxCharCount, syndicationItem.Summary.Text.Length - maxCharCount);
                    content = content + "...";
                }


                _newsFeed.Add(new NewsFeedViewModel(syndicationItem.Id, content, syndicationItem.Categories.Count > 0 ? syndicationItem.Categories[0].Label : "null", syndicationItem.Authors[0].Name, syndicationItem.Title.Text));
            }

            rssurl = @"https://www.youtube.com/feeds/videos.xml?channel_id=UCOWs5Rx9ot7p10mqYyzjyUA";
            reader = XmlReader.Create(rssurl);
            feed = SyndicationFeed.Load(reader);
            reader.Close();

            if (feed == null)
                return;

            foreach (var syndicationItem in feed.Items) {
                var youtubeID = syndicationItem.Id.Replace("yt:video:", "");
                var url = "https://www.youtube.com/watch?v=" + youtubeID;

                var image = SaveThumbnail(youtubeID);

                _videoFeed.Add(new VideoFeedViewModel(url, syndicationItem.Title.Text, image));
            }
        }

        private IBitmap SaveThumbnail(string youtubeID) {

            var savePath = Path.Combine(Platform.CacheDirectory, "videoFeed", youtubeID + ".png");

            if (!Directory.Exists(Path.Combine(Platform.CacheDirectory, "videoFeed"))) {
                Directory.CreateDirectory(Path.Combine(Platform.CacheDirectory, "videoFeed"));
            }

            if (File.Exists(savePath))
                return new Bitmap(savePath);

            var thumbnail = "https://i4.ytimg.com/vi/" + youtubeID + "/hqdefault.jpg";

            using (WebClient client = new WebClient()) {
                client.DownloadFile(new Uri(thumbnail), savePath);
            }

            return new Bitmap(savePath);
        }

        private void ShellOnSolutionChanged(object sender, SolutionChangedEventArgs solutionChangedEventArgs) {
            var newProject = new RecentProject {
                Name = solutionChangedEventArgs.NewValue.Name,
                Path = solutionChangedEventArgs.NewValue.CurrentDirectory
            };

            if (RecentProjectsCollection.RecentProjects == null)
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


        public ObservableCollection<VideoFeedViewModel> VideoFeed
        {
            get { return _videoFeed; }
            set { this.RaiseAndSetIfChanged(ref _videoFeed, value); }
        }
    }
}
