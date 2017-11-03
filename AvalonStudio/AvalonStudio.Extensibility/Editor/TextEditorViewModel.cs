using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Documents;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Extensibility.Editor
{
    public class TextEditorViewModel : EditorViewModel
    {
        private string _zoomLevelText;
        private double _fontSize;
        private double _zoomLevel;
        private double _visualFontSize;
        private IShell _shell;
        private ITextDocument _document;
        private string _sourceText;

        public TextEditorViewModel(ISourceFile file) : base(file)
        {
            _shell = IoC.Get<IShell>();
            _visualFontSize = _fontSize = 14;
            _zoomLevel = 1;
            Title = file.Name;
            //ZoomLevel = _shell.GlobalZoomLevel;
        }

        public void Save ()
        {
            _document?.Save();
        }

        public ITextDocument Document
        {
            get { return _document; }
            set { this.RaiseAndSetIfChanged(ref _document, value); }
        }

        public string SourceText
        {
            get { return _sourceText; }
            set { this.RaiseAndSetIfChanged(ref _sourceText, value); }
        }

        public string FontFamily
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    case Platforms.PlatformID.Win32NT:
                        return "Consolas";

                    default:
                        return "Inconsolata";
                }
            }
        }

        public double FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    InvalidateVisualFontSize();
                }
            }
        }

        public double ZoomLevel
        {
            get
            {
                return _zoomLevel;
            }
            set
            {
                if (value != _zoomLevel)
                {
                    _zoomLevel = value;
                    //_shell.GlobalZoomLevel = value;
                    InvalidateVisualFontSize();

                    ZoomLevelText = $"{ZoomLevel:0} %";
                }
            }
        }

        public string ZoomLevelText
        {
            get { return _zoomLevelText; }
            set { this.RaiseAndSetIfChanged(ref _zoomLevelText, value); }
        }

        public double VisualFontSize
        {
            get { return _visualFontSize; }
            set { this.RaiseAndSetIfChanged(ref _visualFontSize, value); }
        }

        private void InvalidateVisualFontSize()
        {
            VisualFontSize = (ZoomLevel / 100) * FontSize;
        }
    }
}
