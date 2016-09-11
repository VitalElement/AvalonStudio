using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class NewsFeedViewModel : ViewModel {
        private string _title;
        private string _author;
        private string _category;
        private string _url;
        private string _content;

        public NewsFeedViewModel(string url, string content, string category, string author, string title) {
            _url = url;
            _content = content;
            _category = category;
            _author = author;
            _title = title;

            ClickCommand = ReactiveCommand.Create();

            ClickCommand.Subscribe(_ => {
                System.Diagnostics.Process.Start(url);
            });
        }

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }

        public string Author
        {
            get { return _author; }
            set { this.RaiseAndSetIfChanged(ref _author, value); }
        }

        public string Category
        {
            get { return _category; }
            set { this.RaiseAndSetIfChanged(ref _category, value); }
        }

        public string Content
        {
            get { return _content; }
            set { this.RaiseAndSetIfChanged(ref _content, value); }
        }

        public ReactiveCommand<object> ClickCommand { get; }
    }
}
