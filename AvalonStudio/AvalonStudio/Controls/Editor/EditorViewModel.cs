namespace AvalonStudio.Controls
{
    using Languages;
    using MVVM;
    using Perspex;
    using Perspex.Input;
    using Platforms;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TextEditor;
    using TextEditor.Document;
    using TextEditor.Rendering;

    public class EditorViewModel : ViewModel<EditorModel>
    {
        private List<IBackgroundRenderer> languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();
        private List<IDocumentLineTransformer> languageServiceDocumentLineTransformers = new List<IDocumentLineTransformer>();

        #region Constructors
        public EditorViewModel(EditorModel model) : base(model)
        {
            this.highlightingData = new ObservableCollection<SyntaxHighlightingData>();

            BeforeTextChangedCommand = ReactiveCommand.Create();
            BeforeTextChangedCommand.Subscribe(model.OnBeforeTextChanged);

            TextChangedCommand = ReactiveCommand.Create();
            TextChangedCommand.Subscribe(model.OnTextChanged);

            SaveCommand = ReactiveCommand.Create();
            SaveCommand.Subscribe((param) => Save());

            CloseCommand = ReactiveCommand.Create();
            CloseCommand.Subscribe(_ =>
            {
                Save();
                ShellViewModel.Instance.Documents.Remove(this);
                Model.ShutdownBackgroundWorkers();
            });

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
                    ShellViewModel.Instance.InvalidateErrors();
                };

                model.CodeCompletionRequestCompleted += (s, ee) =>
                {
                    Intellisense.SetCompletionResults(model.CodeCompletionResults);
                };

                this.RaisePropertyChanged(nameof(TextDocument));
                this.RaisePropertyChanged(nameof(Title));
            };

            model.TextChanged += (sender, e) =>
            {
                this.RaisePropertyChanged(nameof(Title));
            };

            this.intellisense = new IntellisenseViewModel(model, this);

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
        }

        ~EditorViewModel()
        {
            Model.ShutdownBackgroundWorkers();
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

        public IntellisenseViewModel intellisense;
        public IntellisenseViewModel Intellisense
        {
            get { return intellisense; }
            set { this.RaiseAndSetIfChanged(ref intellisense, value); }
        }


        public TextDocument TextDocument
        {
            get
            {
                return Model.TextDocument;
            }
        }

        public void GotoPosition(int line, int column)
        {
            CaretIndex = TextDocument.GetOffset(line, column);
        }

        public void GotoOffset(int offset)
        {
            CaretIndex = offset;
        }

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref caretIndex, value);
                ShellViewModel.Instance.StatusBar.Offset = value;

                if (value >= 0)
                {
                    var location = TextDocument.GetLocation(value);
                    ShellViewModel.Instance.StatusBar.LineNumber = location.Line;
                    ShellViewModel.Instance.StatusBar.Column = location.Column;
                }
            }
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
        public bool UpdateHoverProbe(int offset)
        {
            bool result = false;

            if (offset != -1)
            {
                var symbol = Model.LanguageService?.GetSymbol(Model.ProjectFile, EditorModel.UnsavedFiles, offset);

                if (symbol != null)
                {
                    switch (symbol.Kind)
                    {
                        case CursorKind.CompoundStatement:
                        case CursorKind.NoDeclarationFound:
                        case CursorKind.NotImplemented:
                        case CursorKind.FirstDeclaration:
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

        public void OnKeyUp(KeyEventArgs e)
        {
            Intellisense.OnKeyUp(e);
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            Intellisense.OnKeyDown(e);
        }


        public void OnTextInput(TextInputEventArgs e)
        {
            Intellisense.OnTextInput(e);
        }

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
            if (Model?.LanguageService != null&& Model.Editor != null)
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
        #endregion

        #region Public Methods
        public void Save()
        {
            Model.Save();

            this.RaisePropertyChanged(nameof(Title));
        }
        #endregion
    }
}
