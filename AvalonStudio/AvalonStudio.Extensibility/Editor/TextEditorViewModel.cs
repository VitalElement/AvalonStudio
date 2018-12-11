using Avalonia;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Projects;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System;
using System.Threading.Tasks;
using AvalonStudio.Extensibility.Shell;
using System.Reactive.Linq;

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
        private bool _isFocused;
        private ISegment _selection;
        private IList<ITextEditorInputHelper> _inputHelpers;
        private int _lastLineNumber;
        private IStatusBar _statusBar;

        public event EventHandler<TooltipDataRequestEventArgs> TooltipContentRequested;

        public TextEditorViewModel(ITextDocument document, ISourceFile file) : base(file)
        {
            _lastLineNumber = -1;
            _visualFontSize = _fontSize = 14;
            _zoomLevel = 1;

            if (_studio != null)
            {
                ZoomLevel = _studio.GlobalZoomLevel;
            }

            _document = document;

            _inputHelpers = new List<ITextEditorInputHelper>();

            _inputHelpers.Add(new DefaultIndentationInputHelper());

            this.WhenAnyValue(x => x.Line).Subscribe(lineNumber =>
            {
                if (lineNumber != _lastLineNumber && lineNumber > 0)
                {
                    var line = Document.Lines[Line];

                    var text = Document.GetText(line);

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        foreach (var helper in InputHelpers)
                        {
                            helper.CaretMovedToEmptyLine(this);
                        }
                    }

                    _lastLineNumber = lineNumber;
                }
            });

            this.WhenAnyValue(x => x.Offset).ObserveOn(RxApp.MainThreadScheduler).Subscribe(x =>
            {
                _statusBar?.SetTextPosition(Offset, Line, Column);
            });

            _statusBar = IoC.Get<IStatusBar>();
        }

        ~TextEditorViewModel()
        {
        }

        public void SetCursorQuiet(int offset)
        {
            _offset = offset;

            var location = Document.GetLocation(Offset);

            _line = location.Line;
            _column = location.Column;
        }

        public IList<ITextEditorInputHelper> InputHelpers
        {
            get { return _inputHelpers; }
            set { this.RaiseAndSetIfChanged(ref _inputHelpers, value); }
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

        public ISegment Selection
        {
            get { return _selection; }
            set { this.RaiseAndSetIfChanged(ref _selection, value); }
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
                    IoC.Get<IStudio>().GlobalZoomLevel = value;
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

        public bool IsFocused
        {
            get { return _isFocused; }
            set { this.RaiseAndSetIfChanged(ref _isFocused, value); }
        }

        private void InvalidateVisualFontSize()
        {
            VisualFontSize = (ZoomLevel / 100) * FontSize;
        }

        public override void OnSelected()
        {
            base.OnSelected();

            IsFocused = true;
        }

        public void Focus()
        {
            IsFocused = true;
        }

        void ITextDocumentTabViewModel.GotoPosition(int line, int column)
        {
            Offset = Document.GetOffset(line, column);
        }

        void ITextDocumentTabViewModel.GotoOffset(int offset)
        {
            Offset = offset;
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

        public virtual bool OnBeforeTextEntered(string text)
        {
            bool handled = false;

            foreach (var helper in InputHelpers)
            {
                if (helper.BeforeTextInput(this, text))
                {
                    handled = true;
                }
            }

            return handled;
        }

        public virtual bool OnTextEntered(string text)
        {
            bool handled = false;

            foreach (var helper in InputHelpers)
            {
                if (helper.AfterTextInput(this, text))
                {
                    handled = true;
                }
            }

            return handled;
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

        public virtual async Task<object> GetToolTipContentAsync(int offset)
        {
            var args = new TooltipDataRequestEventArgs(this, offset);

            TooltipContentRequested?.Invoke(this, args);

            if (args.GetViewModelAsyncTask != null)
            {
                return await args.GetViewModelAsyncTask(this, offset);
            }
            else
            {
                return null;
            }
        }
    }
}
