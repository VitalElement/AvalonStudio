namespace AvalonStudio.Controls
{
    using Documents;
    using Languages;
    using MVVM;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Threading;
    using Platforms;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reactive.Disposables;
    using TextEditor;
    using TextEditor.Document;
    using TextEditor.Rendering;
    using ViewModels;
    using Projects;
    using Shell;
    using System.Threading.Tasks;
    using System.Threading;
    using Debugging;
    using Extensibility;
    using Extensibility.Languages;
    using System.Linq;

    public class EditorViewModel : ViewModel<EditorModel>, IEditor
    {
        private List<IBackgroundRenderer> languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();
        private List<IDocumentLineTransformer> languageServiceDocumentLineTransformers = new List<IDocumentLineTransformer>();
        private CompositeDisposable disposables;


        #region Constructors
        public EditorViewModel(EditorModel model) : base(model)
        {
            disposables = new CompositeDisposable();
            this.highlightingData = new ObservableCollection<SyntaxHighlightingData>();

            BeforeTextChangedCommand = ReactiveCommand.Create();
            disposables.Add(BeforeTextChangedCommand.Subscribe(model.OnBeforeTextChanged));

            TextChangedCommand = ReactiveCommand.Create();
            disposables.Add(TextChangedCommand.Subscribe(model.OnTextChanged));

            SaveCommand = ReactiveCommand.Create();
            disposables.Add(SaveCommand.Subscribe((param) => Save()));

            CloseCommand = ReactiveCommand.Create();
            disposables.Add(CloseCommand.Subscribe(_ =>
            {
                Save();
                Model.ShutdownBackgroundWorkers();
                Model.UnRegisterLanguageService();

                if (ShellViewModel.Instance.DocumentTabs.TemporaryDocument == this)
                {
                    ShellViewModel.Instance.DocumentTabs.TemporaryDocument = null;
                }

                ShellViewModel.Instance.InvalidateErrors();

                Model.Dispose();
                Intellisense.Dispose();
                disposables.Dispose();

                Model.TextDocument = null;
                ShellViewModel.Instance.DocumentTabs.Documents.Remove(this);
            }));

            AddWatchCommand = ReactiveCommand.Create();
            disposables.Add(AddWatchCommand.Subscribe(_ =>
            {
                IoC.Get<IWatchList>()?.AddWatch(WordAtCaret);
            }));

            tabCharacter = "    ";

            model.DocumentLoaded += (sender, e) =>
            {
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

                    languageServiceDocumentLineTransformers.AddRange(model.LanguageService.GetDocumentLineTransformers(model.ProjectFile));

                    foreach (var textTransformer in languageServiceDocumentLineTransformers)
                    {
                        DocumentLineTransformers.Add(textTransformer);
                    }
                }

                model.CodeAnalysisCompleted += (s, ee) =>
                {
                    Diagnostics = model.CodeAnalysisResults.Diagnostics;
                    HighlightingData = new ObservableCollection<SyntaxHighlightingData>(model.CodeAnalysisResults.SyntaxHighlightingData);

                    IndexItems = new ObservableCollection<IndexEntry>(model.CodeAnalysisResults.IndexItems);
                    selectedIndexEntry = IndexItems.FirstOrDefault();
                    this.RaisePropertyChanged(nameof(SelectedIndexEntry));

                    ShellViewModel.Instance.InvalidateErrors();
                };

                TextDocument = model.TextDocument;
                this.RaisePropertyChanged(nameof(Title));
            };

            model.TextChanged += (sender, e) =>
            {
                if (ShellViewModel.Instance.DocumentTabs.TemporaryDocument == this)
                {
                    Dock = Dock.Left;

                    ShellViewModel.Instance.DocumentTabs.TemporaryDocument = null;
                }

                this.RaisePropertyChanged(nameof(Title));
            };

            this.intellisense = new IntellisenseViewModel(model, this);
            this.completionAdvice = new CompletionAdviceViewModel();

            documentLineTransformers = new ObservableCollection<IDocumentLineTransformer>();

            backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
            backgroundRenderers.Add(new SelectedLineBackgroundRenderer());

            DebugLineHighlighter = new SelectedDebugLineBackgroundRenderer();
            backgroundRenderers.Add(DebugLineHighlighter);

            backgroundRenderers.Add(new ColumnLimitBackgroundRenderer());
            wordAtCaretHighlighter = new SelectedWordBackgroundRenderer();
            backgroundRenderers.Add(wordAtCaretHighlighter);
            backgroundRenderers.Add(new SelectionBackgroundRenderer());

            margins = new ObservableCollection<TextViewMargin>();

            ShellViewModel.Instance.StatusBar.InstanceCount++;

            dock = Dock.Right;
        }


        ~EditorViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ShellViewModel.Instance.StatusBar.InstanceCount--;
            });
            Model.ShutdownBackgroundWorkers();

            System.Console.WriteLine(("Editor VM Destructed."));
        }
        #endregion

        public SelectedDebugLineBackgroundRenderer DebugLineHighlighter;
        private SelectedWordBackgroundRenderer wordAtCaretHighlighter;

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

        private ObservableCollection<IDocumentLineTransformer> documentLineTransformers;
        public ObservableCollection<IDocumentLineTransformer> DocumentLineTransformers
        {
            get { return documentLineTransformers; }
            set { this.RaiseAndSetIfChanged(ref documentLineTransformers, value); }
        }

        private ObservableCollection<TextViewMargin> margins;
        public ObservableCollection<TextViewMargin> Margins
        {
            get { return margins; }
            set { this.RaiseAndSetIfChanged(ref margins, value); }
        }

        private string wordAtCaret;
        public string WordAtCaret
        {
            get { return wordAtCaret; }
            set { this.RaiseAndSetIfChanged(ref wordAtCaret, value); wordAtCaretHighlighter.SelectedWord = value; }
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
            get { return caretLocation; }
            set
            {
                this.RaiseAndSetIfChanged(ref caretLocation, value);

                if (!Intellisense.IsVisible)
                {
                    Intellisense.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);
                    CompletionAdvice.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);
                    // TODO implement scroll offset changed? To set intellisense position.
                }
            }
        }        

        public string FontFamily
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    case PlatformID.Unix:
                        return "Monospace";

                    default:
                        return "Consolas";
                }
            }
        }

        private CompletionAdviceViewModel completionAdvice;
        public CompletionAdviceViewModel CompletionAdvice
        {
            get { return completionAdvice; }
            set { this.RaiseAndSetIfChanged(ref completionAdvice, value); }
        }


        private IntellisenseViewModel intellisense;
        public IntellisenseViewModel Intellisense
        {
            get { return intellisense; }
            set { this.RaiseAndSetIfChanged(ref intellisense, value); }
        }


        private TextDocument textDocument;
        public TextDocument TextDocument
        {
            get { return textDocument; }
            set { this.RaiseAndSetIfChanged(ref textDocument, value); }
        }

        public void GotoPosition(int line, int column)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                CaretIndex = TextDocument.GetOffset(line, column);
            });
        }

        public void GotoOffset(int offset)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                CaretIndex = offset;
            });
        }

        public IndexEntry GetSelectIndexEntryByOffset(int offset)
        {
            IndexEntry selectedEntry = null;

            if (IndexItems != null && IndexItems.Count > 0)
            {
                int i = IndexItems.Count - 1;

                while (i >= 0)
                {
                    var entry = IndexItems[i];

                    if (offset >= entry.Offset)
                    {
                        selectedEntry = entry;
                        break;
                    }

                    i--;
                }
            }

            return selectedEntry;
        }

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref caretIndex, value);
                ShellViewModel.Instance.StatusBar.Offset = value;

                if (value >= 0 && TextDocument != null)
                {
                    var location = TextDocument.GetLocation(value);
                    ShellViewModel.Instance.StatusBar.LineNumber = location.Line;
                    ShellViewModel.Instance.StatusBar.Column = location.Column;
                }

                selectedIndexEntry = GetSelectIndexEntryByOffset(value);
                this.RaisePropertyChanged(nameof(SelectedIndexEntry));
            }
        }

        private WatchListViewModel debugHoverProbe;
        public WatchListViewModel DebugHoverProbe
        {
            get { return debugHoverProbe; }
            set { this.RaiseAndSetIfChanged(ref debugHoverProbe, value); }
        }

        private string GetWordAtOffset(int offset)
        {
            string result = string.Empty;

            if (offset >= 0 && TextDocument.TextLength > offset)
            {
                int start = offset;

                var currentChar = TextDocument.GetCharAt(offset);
                char prevChar = '\0';

                if (offset > 0)
                {
                    prevChar = TextDocument.GetCharAt(offset - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != TextUtilities.CharacterClass.LineTerminator && prevChar != ' ' && TextUtilities.GetCharacterClass(prevChar) != TextUtilities.CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(TextDocument, offset, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                }

                int end = TextUtilities.GetNextCaretPosition(TextDocument, start, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    string word = TextDocument.GetText(start, end - start).Trim();

                    if (TextUtilities.IsSymbol(word))
                    {
                        result = word;
                    }
                }
            }

            return result;
        }

        public async Task<bool> UpdateDebugHoverProbe(int offset)
        {
            bool result = false;

            //ShellViewModel.Instance.Console.WriteLine("Injection of DebugProb behaviour required");

            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Debug)
            {
                var expression = GetWordAtOffset(offset);

                if (expression != string.Empty)
                {
                    var debugManager = IoC.Get<IDebugManager>();

                    
                    var evaluatedExpression = await debugManager.ProbeExpressionAsync(expression);

                    if (evaluatedExpression != null)
                    {
                        DebugHoverProbe = new WatchListViewModel(debugManager);
                        DebugHoverProbe.AddExistingWatch(evaluatedExpression);
                        result = true;
                    }
                }
            }

            return result;
        }


        private SymbolViewModel hoverProbe;
        public SymbolViewModel HoverProbe
        {
            get { return hoverProbe; }
            set { this.RaiseAndSetIfChanged(ref hoverProbe, value); }
        }
        /// <summary>
        /// Updates the contents of the HoverProbe (Code tooltip) to display content for specified offset.
        /// </summary>
        /// <param name="offset">the offset inside text document to retreive data for.</param>
        /// <returns>true if data was found.</returns>
        public async Task<bool> UpdateHoverProbeAsync(int offset)
        {
            bool result = false;

            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
            {
                var symbol = await Model.LanguageService?.GetSymbolAsync(Model.ProjectFile, EditorModel.UnsavedFiles, offset);

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
                            break;

                        default:

                            HoverProbe = new SymbolViewModel(symbol);
                            result = true;
                            break;
                    }
                }
            }

            return result;
        }

        private IndexEntry selectedIndexEntry;
        public IndexEntry SelectedIndexEntry
        {
            get { return selectedIndexEntry; }
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


        private ObservableCollection<SyntaxHighlightingData> highlightingData;
        public ObservableCollection<SyntaxHighlightingData> HighlightingData
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

        public string Title
        {
            get
            {
                string result = Model.ProjectFile?.Name;

                if (Model.IsDirty)
                {
                    result += '*';
                }

                return result;
            }
        }
        #endregion

        public TextSegment GetSelection()
        {
            TextSegment result = null;

            if (Model.Editor != null)
            {
                if (Model.Editor.SelectionStart < Model.Editor.SelectionEnd)
                {
                    result = new TextSegment() { StartOffset = Model.Editor.SelectionStart, EndOffset = Model.Editor.SelectionEnd };
                }
                else
                {
                    result = new TextSegment() { StartOffset = Model.Editor.SelectionEnd, EndOffset = Model.Editor.SelectionStart };
                }
            }

            return result;
        }

        public void SetSelection(TextSegment segment)
        {
            if (Model.Editor != null)
            {
                Model.Editor.SelectionStart = segment.StartOffset;
                Model.Editor.SelectionEnd = segment.EndOffset;
            }
        }

        public void Comment()
        {
            if (Model?.LanguageService != null && Model.Editor != null)
            {
                var selection = GetSelection();

                if (selection != null)
                {
                    var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                    anchors.Add(selection);

                    CaretIndex = Model.LanguageService.Comment(TextDocument, selection, CaretIndex);

                    SetSelection(selection);
                }

                Model.Editor.Focus();
            }
        }

        public void UnComment()
        {
            if (Model?.LanguageService != null && Model.Editor != null)
            {
                var selection = GetSelection();

                if (selection != null)
                {
                    var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                    anchors.Add(selection);

                    CaretIndex = Model.LanguageService.UnComment(TextDocument, selection, CaretIndex);

                    SetSelection(selection);
                }

                Model.Editor.Focus();
            }
        }

        public void Undo()
        {
            TextDocument?.UndoStack.Undo();
        }

        public void Redo()
        {
            TextDocument.UndoStack.Redo();
        }

        private void FormatAll()
        {
            if (Model?.LanguageService != null)
            {
                CaretIndex = Model.LanguageService.Format(TextDocument, 0, (uint)TextDocument.TextLength, CaretIndex);
            }
        }

        private Dock dock;
        public Dock Dock
        {
            get { return dock; }
            set { this.RaiseAndSetIfChanged(ref dock, value); }
        }



        public TextLocation CaretTextLocation
        {
            get
            {
                return TextDocument.GetLocation(caretIndex);
            }
        }

        #region Commands
        public ReactiveCommand<object> BeforeTextChangedCommand { get; private set; }
        public ReactiveCommand<object> TextChangedCommand { get; private set; }
        public ReactiveCommand<object> SaveCommand { get; private set; }
        public ReactiveCommand<object> CloseCommand { get; }

        // todo this menu item and command should be injected via debugging module.
        public ReactiveCommand<object> AddWatchCommand { get; }

        public ISourceFile ProjectFile
        {
            get
            {
                return Model.ProjectFile;
            }
        }
        #endregion

        #region Public Methods
        public void Save()
        {
            Model.Save();

            this.RaisePropertyChanged(nameof(Title));
        }

        public void ClearDebugHighlight()
        {
            DebugLineHighlighter.Line = -1;
        }
        #endregion
    }
}
