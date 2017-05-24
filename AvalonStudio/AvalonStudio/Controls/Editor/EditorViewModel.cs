using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using AvaloniaEdit.Rendering;
using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Controls.Standard.ErrorList;
using System.Linq;
using System.Collections.Generic;

namespace AvalonStudio.Controls
{
    public class EditorViewModel : DocumentTabViewModel, IEditor
    {
        private readonly CompositeDisposable disposables;

        public ISourceFile ProjectFile { get; set; }

        public void SetDebugHighlight(int line, int startColumn, int endColumn)
        {
            _editor?.SetDebugHighlight(line, startColumn, endColumn);
        }

        public void Comment()
        {
            _editor?.Comment();
        }

        public void UnComment()
        {
            _editor?.UnComment();
        }

        public void Undo()
        {
            _editor?.Undo();
        }

        public void Redo()
        {
            _editor?.Redo();
        }

        public void OpenFile(ISourceFile file)
        {
            ProjectFile = file;
            SourceFile = file;
            Title = Path.GetFileName(file.Location);
        }

        public void FormatAll()
        {
            _editor?.FormatAll();
        }

        public void ClearDebugHighlight()
        {
            _editor?.ClearDebugHighlight();
        }

        public void GotoOffset(int offset)
        {
            _editor?.GotoOffset(offset);
        }

        #region Constructors

        public EditorViewModel()
        {
            disposables = new CompositeDisposable();

            disposables.Add(CloseCommand.Subscribe(_ =>
            {
                Close();

                IoC.Get<IShell>().InvalidateErrors();
                disposables.Dispose();
            }));

            AddWatchCommand = ReactiveCommand.Create();
            disposables.Add(AddWatchCommand.Subscribe(_ => { IoC.Get<IWatchList>()?.AddWatch(_editor?.GetWordAtOffset(_editor.CaretOffset)); }));

            Dock = Dock.Right;
        }

        ~EditorViewModel()
        {
        }

        public void AttachEditor(IEditor editor)
        {
            _editor = editor;
        }

        private IEditor _editor;

        #endregion Constructors

        #region Properties

        private string tabCharacter;

        public string TabCharacter
        {
            get { return tabCharacter; }
            set { this.RaiseAndSetIfChanged(ref tabCharacter, value); }
        }

        private double lineHeight;

        public double LineHeight
        {
            get { return lineHeight; }
            set { this.RaiseAndSetIfChanged(ref lineHeight, value); }
        }

        private Point caretLocation;

        public Point CaretLocation
        {
            get
            {
                return caretLocation;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref caretLocation, value);
            }
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

        private ISourceFile _sourceFile;

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }


        public void GotoPosition(int line, int column)
        {
            _editor?.GotoPosition(line, column);
        }

        public IndexEntry GetSelectIndexEntryByOffset(int offset)
        {
            IndexEntry selectedEntry = null;

            if (IndexItems != null && IndexItems.Count > 0)
            {
                var i = IndexItems.Count - 1;

                while (i >= 0)
                {
                    var entry = IndexItems[i];

                    if (offset >= entry.Offset && offset < entry.EndOffset)
                    {
                        selectedEntry = entry;
                        break;
                    }
                    else
                    {
                        selectedEntry = null;
                    }

                    i--;
                }
            }

            return selectedEntry;
        }

        public async Task<object> UpdateToolTipAsync(int offset)
        {
            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
            {
                var matching = IoC.Get<IErrorList>().FindDiagnosticsAtOffset(offset).FirstOrDefault();

                if (matching != null)
                {
                    return new ErrorProbeViewModel(matching);
                }

                var symbol = await _editor?.GetSymbolAsync(offset);

                if (symbol != null)
                {
                    switch (symbol.Kind)
                    {
                        case CursorKind.CompoundStatement:
                        case CursorKind.NoDeclarationFound:
                        case CursorKind.NotImplemented:
                        case CursorKind.FirstDeclaration:
                        case CursorKind.InitListExpression:
                        case CursorKind.IntegerLiteral:
                        case CursorKind.ReturnStatement:
                        case CursorKind.WhileStatement:
                        case CursorKind.BinaryOperator:
                            return null;

                        default:
                            return new SymbolViewModel(symbol);
                    }
                }
            }

            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Debug)
            {
                var expression = _editor?.GetWordAtOffset(offset);

                if (expression != string.Empty)
                {
                    var debugManager = IoC.Get<IDebugManager2>();

                    var newToolTip = new DebugHoverProbeViewModel();

                    bool result = newToolTip.AddWatch(expression);

                    return newToolTip;
                }
            }

            return null;
        }

        private IndexEntry selectedIndexEntry;

        public IndexEntry SelectedIndexEntry
        {
            get
            {
                return selectedIndexEntry;
            }
            set
            {
                if (value != null && value != selectedIndexEntry)
                {
                    selectedIndexEntry = value;
                    GotoOffset(selectedIndexEntry.Offset);

                    this.RaisePropertyChanged(nameof(SelectedIndexEntry));
                }
            }
        }

        private ObservableCollection<IndexEntry> indexItems;

        public ObservableCollection<IndexEntry> IndexItems
        {
            get { return indexItems; }
            set { this.RaiseAndSetIfChanged(ref indexItems, value); }
        }

        private ObservableCollection<OffsetSyntaxHighlightingData> highlightingData;

        public ObservableCollection<OffsetSyntaxHighlightingData> HighlightingData
        {
            get { return highlightingData; }
            set { this.RaiseAndSetIfChanged(ref highlightingData, value); }
        }

        private List<Diagnostic> diagnostics;

        public List<Diagnostic> Diagnostics
        {
            get { return diagnostics; }
            set { this.RaiseAndSetIfChanged(ref diagnostics, value); }
        }

        #endregion Properties

        #region Commands

        public ReactiveCommand<object> BeforeTextChangedCommand { get; }
        public ReactiveCommand<object> TextChangedCommand { get; }
        public ReactiveCommand<object> SaveCommand { get; }

        // todo this menu item and command should be injected via debugging module.
        public ReactiveCommand<object> AddWatchCommand { get; }

        private int _caretOffset;

        public int CaretOffset
        {
            get { return _caretOffset; }
            set
            {
                this.RaiseAndSetIfChanged(ref _caretOffset, value);
                ShellViewModel.Instance.StatusBar.Offset = value;

                /*selectedIndexEntry = GetSelectIndexEntryByOffset(value);
                this.RaisePropertyChanged(nameof(SelectedIndexEntry));*/
            }
        }

        private int _line;

        public int Line
        {
            get { return _line; }
            set
            {
                this.RaiseAndSetIfChanged(ref _line, value);

                ShellViewModel.Instance.StatusBar.LineNumber = value;
            }
        }

        private int _column;

        public int Column
        {
            get { return _column; }
            set
            {
                this.RaiseAndSetIfChanged(ref _column, value);

                ShellViewModel.Instance.StatusBar.Column = value;
            }
        }


        private string _languageServiceName;

        public string LanguageServiceName
        {
            get { return _languageServiceName; }
            set
            {
                this.RaiseAndSetIfChanged(ref _languageServiceName, value);

                ShellViewModel.Instance.StatusBar.Language = value;
            }
        }



        #endregion Commands

        #region Public Methods

        private bool ignoreFileModifiedEvents = false;

        public void Save()
        {
            ignoreFileModifiedEvents = true;

            FormatAll();

            _editor.Save();

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ignoreFileModifiedEvents = false;
                IsDirty = false;
            });
        }

        public void Close()
        {
            _editor?.Close();

            IoC.Get<IShell>().CloseDocument(ProjectFile);
        }

        public async Task<Symbol> GetSymbolAsync(int offset)
        {
            return await _editor?.GetSymbolAsync(offset);
        }

        public string GetWordAtOffset(int offset)
        {
            return _editor?.GetWordAtOffset(offset);
        }

        public void InstallBackgroundRenderer(IBackgroundRenderer backgroundRenderer)
        {
            _editor.InstallBackgroundRenderer(backgroundRenderer);
        }

        public void InstallVisualLineTransformer(IVisualLineTransformer transformer)
        {
            _editor.InstallVisualLineTransformer(transformer);
        }

        public TextDocument GetDocument()
        {
            return _editor.GetDocument();
        }

        #endregion Public Methods
    }
}