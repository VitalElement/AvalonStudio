using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Rendering;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class EditorViewModel : DocumentTabViewModel, IEditor
    {
        private readonly CompositeDisposable disposables;

        private readonly List<IBackgroundRenderer> languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();

        private readonly List<IVisualLineTransformer> languageServiceDocumentLineTransformers = new List<IVisualLineTransformer>();

        private readonly SelectedWordBackgroundRenderer wordAtCaretHighlighter;

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
                _editor?.Close();

                IoC.Get<IShell>().InvalidateErrors();
                disposables.Dispose();
            }));

            //AddWatchCommand = ReactiveCommand.Create(this.WhenAny(x => x.WordAtCaret, (word) => !string.IsNullOrEmpty(word.Value)));
            //disposables.Add(AddWatchCommand.Subscribe(_ => { IoC.Get<IWatchList>()?.AddWatch(WordAtCaret); }));

            //tabCharacter = "    ";

            /*model.DocumentChanged += (sender, e) =>
            {
                model.ProjectFile.FileModifiedExternally -= ProjectFile_FileModifiedExternally;
               // Model.Editor.CaretChangedByPointerClick -= Editor_CaretChangedByPointerClick;

                foreach (var bgRenderer in languageServiceBackgroundRenderers)
                {
                    BackgroundRenderers.Remove(bgRenderer);
                }

                languageServiceBackgroundRenderers.Clear();

                foreach (var transformer in languageServiceDocumentLineTransformers)
                {
                    DocumentLineTransformers.Remove(transformer);
                }

                languageServiceDocumentLineTransformers.Clear();

                if (model.LanguageService != null)
                {
                    languageServiceBackgroundRenderers.AddRange(model.LanguageService.GetBackgroundRenderers(model.ProjectFile));

                    foreach (var bgRenderer in languageServiceBackgroundRenderers)
                    {
                        BackgroundRenderers.Add(bgRenderer);
                    }

                    languageServiceDocumentLineTransformers.AddRange(
                        model.LanguageService.GetDocumentLineTransformers(model.ProjectFile));

                    foreach (var textTransformer in languageServiceDocumentLineTransformers)
                    {
                        DocumentLineTransformers.Add(textTransformer);
                    }

                    intellisenseManager = new IntellisenseManager(Model, Intellisense, Intellisense.CompletionAssistant, model.LanguageService, model.ProjectFile);

                    EventHandler<KeyEventArgs> tunneledKeyUpHandler = (send, ee) =>
                    {
                        if (caretIndex > 0)
                        {
                            intellisenseManager.OnKeyUp(ee, CaretIndex, CaretTextLocation.Line, CaretTextLocation.Column);
                        }
                    };

                    EventHandler<KeyEventArgs> tunneledKeyDownHandler = (send, ee) =>
                    {
                        if (caretIndex > 0)
                        {
                            intellisenseManager.OnKeyDown(ee, CaretIndex, CaretTextLocation.Line, CaretTextLocation.Column);
                        }
                    };

                    EventHandler<TextInputEventArgs> tunneledTextInputHandler = (send, ee) =>
                    {
                        if (caretIndex > 0)
                        {
                            intellisenseManager.OnTextInput(ee, CaretIndex, CaretTextLocation.Line, CaretTextLocation.Column);
                        }
                    };

                    //Model.Editor.CaretChangedByPointerClick += Editor_CaretChangedByPointerClick;

                    disposables.Add(Model.AddHandler(InputElement.KeyDownEvent, tunneledKeyDownHandler, RoutingStrategies.Tunnel));
                    disposables.Add(Model.AddHandler(InputElement.KeyUpEvent, tunneledKeyUpHandler, RoutingStrategies.Tunnel));
                    disposables.Add(Model.AddHandler(InputElement.TextInputEvent, tunneledTextInputHandler, RoutingStrategies.Bubble));
                }

                model.CodeAnalysisCompleted += (s, ee) =>
                {
                    Diagnostics = model.CodeAnalysisResults.Diagnostics;

                    HighlightingData =
                        new ObservableCollection<OffsetSyntaxHighlightingData>(model.CodeAnalysisResults.SyntaxHighlightingData);

                    IndexItems = new ObservableCollection<IndexEntry>(model.CodeAnalysisResults.IndexItems);
                    selectedIndexEntry = IndexItems.FirstOrDefault();
                    this.RaisePropertyChanged(nameof(SelectedIndexEntry));

                    ShellViewModel.Instance.InvalidateErrors();
                };

                model.ProjectFile.FileModifiedExternally += ProjectFile_FileModifiedExternally;

                TextDocument = model.Document;

                Title = Model.ProjectFile.Name;
            };

            model.TextChanged += (sender, e) =>
            {
                if (ShellViewModel.Instance.DocumentTabs.TemporaryDocument == this)
                {
                    Dock = Dock.Left;

                    ShellViewModel.Instance.DocumentTabs.TemporaryDocument = null;
                }

                IsDirty = model.IsDirty;
            };*/

            documentLineTransformers = new ObservableCollection<IVisualLineTransformer>();

            backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();

            backgroundRenderers.Add(new ColumnLimitBackgroundRenderer());
            wordAtCaretHighlighter = new SelectedWordBackgroundRenderer();
            backgroundRenderers.Add(wordAtCaretHighlighter);
            backgroundRenderers.Add(new SelectionBackgroundRenderer());

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

        private ObservableCollection<IBackgroundRenderer> backgroundRenderers;

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return backgroundRenderers; }
            set { this.RaiseAndSetIfChanged(ref backgroundRenderers, value); }
        }

        private ObservableCollection<IVisualLineTransformer> documentLineTransformers;

        public ObservableCollection<IVisualLineTransformer> DocumentLineTransformers
        {
            get { return documentLineTransformers; }
            set { this.RaiseAndSetIfChanged(ref documentLineTransformers, value); }
        }

        private string wordAtCaret;

        public string WordAtCaret
        {
            get
            {
                return wordAtCaret;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref wordAtCaret, value);
                wordAtCaretHighlighter.SelectedWord = value;
            }
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

        /*public void GotoOffset(int offset)
        {
            Dispatcher.UIThread.InvokeAsync(() => { CaretIndex = offset; });
        }*/

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

        private int caretIndex;

        public int CaretIndex
        {
            get
            {
                return caretIndex;
            }
            set
            {
                /*if (TextDocument != null && value > TextDocument.TextLength)
                {
                    value = TextDocument.TextLength - 1;
                }

                bool hasChanged = value != caretIndex;*/

                this.RaiseAndSetIfChanged(ref caretIndex, value);
               /* ShellViewModel.Instance.StatusBar.Offset = value;

                if (value >= 0 && TextDocument != null)
                {
                    var location = TextDocument.GetLocation(value);
                    ShellViewModel.Instance.StatusBar.LineNumber = location.Line;
                    ShellViewModel.Instance.StatusBar.Column = location.Column;
                }

                selectedIndexEntry = GetSelectIndexEntryByOffset(value);
                this.RaisePropertyChanged(nameof(SelectedIndexEntry));*/
            }
        }

        

        public async Task<object> UpdateToolTipAsync(int offset)
        {
             if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
             {
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

        private TextSegmentCollection<Diagnostic> diagnostics;

        public TextSegmentCollection<Diagnostic> Diagnostics
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
        }

        public async Task<Symbol> GetSymbolAsync(int offset)
        {
            return await _editor?.GetSymbolAsync(offset);
        }

        public string GetWordAtOffset(int offset)
        {
            return _editor?.GetWordAtOffset(offset);
        }

        #endregion Public Methods
    }
}