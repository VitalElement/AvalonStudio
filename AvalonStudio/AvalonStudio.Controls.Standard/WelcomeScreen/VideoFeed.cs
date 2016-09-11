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
    public class VideoFeedViewModel : ViewModel {
        private string _title;
        private string _url;
        private IBitmap _image;

        public VideoFeedViewModel(string url,  string title, IBitmap image = null) {
            _url = url;
            _title = title;
            _image = image;

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

        public IBitmap Image
        {
            get { return _image; }
            set { this.RaiseAndSetIfChanged(ref _image, value); }
        }

        public ReactiveCommand<object> ClickCommand { get; }
    }
}
