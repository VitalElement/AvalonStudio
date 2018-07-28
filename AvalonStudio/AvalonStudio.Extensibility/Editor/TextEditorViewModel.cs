using AvalonStudio.Documents;
using AvalonStudio.Projects;
using ReactiveUI;
using System.IO;

namespace AvalonStudio.Extensibility.Editor
{
    public class TextEditorViewModel : EditorViewModel, ITextDocumentTabViewModel, ITextEditor
    {
        private string _zoomLevelText;
        private double _fontSize;
        private double _zoomLevel;
        private double _visualFontSize;
        private ITextDocument _document;
        private int _offset;
        private int _line;
        private int _column;

        public TextEditorViewModel(ITextDocument document, ISourceFile file) : base(file)
        {
            _visualFontSize = _fontSize = 14;
            _zoomLevel = 1;
            ZoomLevel = _studio.GlobalZoomLevel;
            _document = document;
        }

        ~TextEditorViewModel()
        {
        }

        public int Offset
        {
            get { return _offset; }
            set { this.RaiseAndSetIfChanged(ref _offset, value); }
        }

        public int Line
        {
            get { return _line; }
            set { this.RaiseAndSetIfChanged(ref _line, value); }
        }

        public int Column
        {
            get { return _column; }
            set { this.RaiseAndSetIfChanged(ref _column, value); }
        }

        public ITextDocument Document
        {
            get { return _document; }
            set { this.RaiseAndSetIfChanged(ref _document, value); }
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
                    _studio.GlobalZoomLevel = value;
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

        public double VisualFontSize
        {
            get { return _visualFontSize; }
            set { this.RaiseAndSetIfChanged(ref _visualFontSize, value); }
        }

        private void InvalidateVisualFontSize()
        {
            VisualFontSize = (ZoomLevel / 100) * FontSize;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            // how to tell the control to focus.
        }

        public void Focus ()
        {

        }

        void ITextDocumentTabViewModel.GotoPosition(int line, int column)
        {

        }

        void ITextDocumentTabViewModel.GotoOffset(int offset)
        {

        }

        public virtual void Save()
        {
            if (IsDirty)
            {
                if (GlobalSettings.Settings.GetSettings<EditorSettings>().RemoveTrailingWhitespaceOnSave)
                {
                    Document.TrimTrailingWhiteSpace();
                }

                File.WriteAllText(SourceFile.FilePath, Document.Text);
                IsDirty = false;
            }
        }

        public virtual void OnBeforeTextEntered(string text)
        {
        }

        public virtual void OnTextEntered(string text)
        {
        }

        public virtual void OnTextChanged()
        {
            if (!IsReadOnly)
            {
                IsDirty = true;
            }
        }

        public virtual void IndentLine(int lineNumber)
        {
        }
    }
}
