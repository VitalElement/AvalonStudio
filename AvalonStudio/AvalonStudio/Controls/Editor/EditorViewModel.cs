using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class EditorViewModel : DocumentTabViewModel, IEditor
    {
        public SelectedDebugLineBackgroundRenderer DebugLineHighlighter { get; set; }

        private readonly CompositeDisposable disposables;

        private readonly List<IBackgroundRenderer> languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();

        private readonly List<IVisualLineTransformer> languageServiceDocumentLineTransformers = new List<IVisualLineTransformer>();

        private readonly SelectedWordBackgroundRenderer wordAtCaretHighlighter;

        private IntellisenseManager intellisenseManager;

        public ISourceFile ProjectFile { get; set; }

        public void Comment()
        {
            /*if (Model?.LanguageService != null)
            {
                var selection = GetSelection();

                if (selection != null)
                {
                    var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                    anchors.Add(selection);

                    CaretIndex = Model.LanguageService.Comment(TextDocument, selection, CaretIndex);

                    SetSelection(selection);
                }

                Model.Focus();
            }*/
        }

        public void UnComment()
        {
            /*if (Model?.LanguageService != null)
            {
                var selection = GetSelection();

                if (selection != null)
                {
                    var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                    anchors.Add(selection);

                    CaretIndex = Model.LanguageService.UnComment(TextDocument, selection, CaretIndex);

                    SetSelection(selection);
                }

                Model.Focus();
            }*/
        }

        public void Undo()
        {
            //TextDocument?.UndoStack.Undo();
        }

        public void Redo()
        {
           // TextDocument.UndoStack.Redo();
        }

        public TextSegment GetSelection()
        {
            TextSegment result = null;

            /*if (Model.SelectionStart < (Model.SelectionStart + Model.SelectionLength))
            {
                result = new TextSegment { StartOffset = Model.SelectionStart, EndOffset = (Model.SelectionStart + Model.SelectionLength) };
            }
            else
            {
                result = new TextSegment { StartOffset = (Model.SelectionStart + Model.SelectionLength), EndOffset = Model.SelectionStart };
            }*/

            return result;
        }

        public void OpenFile(ISourceFile file, IIntellisenseControl intellisense, ICompletionAssistant completionAssistant)
        {
            SourceFile = file;
        }

        public void SetSelection(TextSegment segment)
        {
               /* Model.SelectionStart = segment.StartOffset;
                Model.SelectionLength = segment.EndOffset - segment.StartOffset;*/
        }

        private void FormatAll()
        {
            /*if (Model?.LanguageService != null && TextDocument != null)
            {
                CaretIndex = Model.LanguageService.Format(TextDocument, 0, (uint)TextDocument.TextLength, CaretIndex);
            }*/
        }

        public void ClearDebugHighlight()
        {
            throw new NotImplementedException();
        }

        public void GotoOffset(int offset)
        {
            throw new NotImplementedException();
        }

        #region Constructors

        public EditorViewModel()
        {
            disposables = new CompositeDisposable();
           // highlightingData = new ObservableCollection<OffsetSyntaxHighlightingData>();

           // SaveCommand = ReactiveCommand.Create();
            ///disposables.Add(SaveCommand.Subscribe(param => Save()));

            disposables.Add(CloseCommand.Subscribe(_ =>
            {
                Editor?.Close();

               // Model.ProjectFile.FileModifiedExternally -= ProjectFile_FileModifiedExternally;
                
                //Model.Editor.CaretChangedByPointerClick -= Editor_CaretChangedByPointerClick;
               // Save();
               // Model.ShutdownBackgroundWorkers();
               // Model.UnRegisterLanguageService();

                intellisenseManager?.Dispose();
                intellisenseManager = null;

               // Diagnostics?.Clear();

                ShellViewModel.Instance.InvalidateErrors();
                
                //Intellisense.Dispose();
                disposables.Dispose();

               // Model.Document = null;
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

            intellisense = new IntellisenseViewModel(this);

            documentLineTransformers = new ObservableCollection<IVisualLineTransformer>();

            backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
            backgroundRenderers.Add(new SelectedLineBackgroundRenderer());

            DebugLineHighlighter = new SelectedDebugLineBackgroundRenderer();
            backgroundRenderers.Add(DebugLineHighlighter);

            backgroundRenderers.Add(new ColumnLimitBackgroundRenderer());
            wordAtCaretHighlighter = new SelectedWordBackgroundRenderer();
            backgroundRenderers.Add(wordAtCaretHighlighter);
            backgroundRenderers.Add(new SelectionBackgroundRenderer());

            //margins = new ObservableCollection<TextViewMargin>();

            Dock = Dock.Right;
        }

        ~EditorViewModel()
        {
        }
        
        public ICodeEditor Editor { get; set; }

        private void Editor_CaretChangedByPointerClick(object sender, EventArgs e)
        {
            if (intellisenseManager != null)
            {
                var location = TextDocument.GetLocation(caretIndex);
                intellisenseManager.SetCursor(caretIndex, location.Line, location.Column, Standard.CodeEditor.CodeEditor.UnsavedFiles.ToList());
            }
        }

        private void ProjectFile_FileModifiedExternally(object sender, EventArgs e)
        {
            if (!ignoreFileModifiedEvents && TextDocument != null)
            {
                /*if (!new FileInfo(Model.ProjectFile.Location).IsFileLocked())
                {
                    using (var fs = System.IO.File.OpenText(Model.ProjectFile.Location))
                    {
                        TextDocument.Text = fs.ReadToEnd();
                    }
                }*/
            }
        }

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

       /* private ObservableCollection<TextViewMargin> margins;

        public ObservableCollection<TextViewMargin> Margins
        {
            get { return margins; }
            set { this.RaiseAndSetIfChanged(ref margins, value); }
        }*/

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

                if (!Intellisense.IsVisible)
                {
                    Intellisense.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);
                }
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

        private ISourceFile _sourceFile;

        public ISourceFile SourceFile
        {
            get { return _sourceFile; }
            set { this.RaiseAndSetIfChanged(ref _sourceFile, value); }
        }


        public void GotoPosition(int line, int column)
        {
            if (textDocument != null)
            {
                Dispatcher.UIThread.InvokeAsync(() => { CaretIndex = TextDocument.GetOffset(line, column); });
            }
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
                if (TextDocument != null && value > TextDocument.TextLength)
                {
                    value = TextDocument.TextLength - 1;
                }

                bool hasChanged = value != caretIndex;

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

        private string GetWordAtOffset(int offset)
        {
            var result = string.Empty;

            if (offset >= 0 && TextDocument.TextLength > offset)
            {
                var start = offset;

                var currentChar = TextDocument.GetCharAt(offset);
                var prevChar = '\0';

                if (offset > 0)
                {
                    prevChar = TextDocument.GetCharAt(offset - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != CharacterClass.LineTerminator && prevChar != ' ' &&
                    TextUtilities.GetCharacterClass(prevChar) != CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(TextDocument, offset, LogicalDirection.Backward,
                        CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(TextDocument, start, LogicalDirection.Forward,
                    CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    var word = TextDocument.GetText(start, end - start).Trim();

                    if (word.IsSymbol())
                    {
                        result = word;
                    }
                }
            }

            return result;
        }

        private object toolTip;

        public object ToolTip
        {
            get { return toolTip; }
            set { this.RaiseAndSetIfChanged(ref toolTip, value); }
        }

        public async Task<bool> UpdateToolTipAsync(int offset)
        {
            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
            {
                /*var matching = Model.CodeAnalysisResults?.Diagnostics.FindSegmentsContaining(offset).FirstOrDefault();

                if (matching != null)
                {
                    ToolTip = new ErrorProbeViewModel(matching);

                    return true;
                }*/
            }

           /* if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor && Model.LanguageService != null)
            {
                var symbol = await Model.LanguageService.GetSymbolAsync(Model.ProjectFile, CodeEditor.CodeEditor.UnsavedFiles.ToList(), offset);

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

                            ToolTip = new SymbolViewModel(symbol);
                            return true;
                    }
                }
            }

            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Debug)
            {
                var expression = GetWordAtOffset(offset);

                if (expression != string.Empty)
                {
                    var debugManager = IoC.Get<IDebugManager2>();

                    var newToolTip = new DebugHoverProbeViewModel();

                    bool result = newToolTip.AddWatch(expression);

                    ToolTip = newToolTip;
                    return result;
                }
            }*/

            return false;
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

            //Model.Save();

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                ignoreFileModifiedEvents = false;
                IsDirty = false;
            });
        }

        #endregion Public Methods
    }
}