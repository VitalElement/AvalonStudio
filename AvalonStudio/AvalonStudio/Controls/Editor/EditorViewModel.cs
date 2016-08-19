using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using AvalonStudio.Debugging;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Languages;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Rendering;
using ReactiveUI;
using AvalonStudio.Utils;

namespace AvalonStudio.Controls
{
	public class EditorViewModel : ViewModel<EditorModel>, IEditor
	{
		public SelectedDebugLineBackgroundRenderer DebugLineHighlighter;
		private readonly CompositeDisposable disposables;

		private Dock dock;
		private readonly List<IBackgroundRenderer> languageServiceBackgroundRenderers = new List<IBackgroundRenderer>();

		private readonly List<IDocumentLineTransformer> languageServiceDocumentLineTransformers =
			new List<IDocumentLineTransformer>();

		private readonly SelectedWordBackgroundRenderer wordAtCaretHighlighter;

		public Dock Dock
		{
			get { return dock; }
			set { this.RaiseAndSetIfChanged(ref dock, value); }
		}


		public TextLocation CaretTextLocation
		{
			get { return TextDocument.GetLocation(caretIndex); }
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

		public TextSegment GetSelection()
		{
			TextSegment result = null;

			if (Model.Editor != null)
			{
				if (Model.Editor.SelectionStart < Model.Editor.SelectionEnd)
				{
					result = new TextSegment { StartOffset = Model.Editor.SelectionStart, EndOffset = Model.Editor.SelectionEnd };
				}
				else
				{
					result = new TextSegment { StartOffset = Model.Editor.SelectionEnd, EndOffset = Model.Editor.SelectionStart };
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

		private void FormatAll()
		{
			if (Model?.LanguageService != null)
			{
				CaretIndex = Model.LanguageService.Format(TextDocument, 0, (uint)TextDocument.TextLength, CaretIndex);
			}
		}

		#region Constructors

		public EditorViewModel(EditorModel model) : base(model)
		{
			disposables = new CompositeDisposable();
			highlightingData = new ObservableCollection<SyntaxHighlightingData>();

			BeforeTextChangedCommand = ReactiveCommand.Create();
			disposables.Add(BeforeTextChangedCommand.Subscribe(model.OnBeforeTextChanged));

			TextChangedCommand = ReactiveCommand.Create();
			disposables.Add(TextChangedCommand.Subscribe(model.OnTextChanged));

			SaveCommand = ReactiveCommand.Create();
			disposables.Add(SaveCommand.Subscribe(param => Save()));

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
			disposables.Add(AddWatchCommand.Subscribe(_ => { IoC.Get<IWatchList>()?.AddWatch(WordAtCaret); }));

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

					languageServiceDocumentLineTransformers.AddRange(
						model.LanguageService.GetDocumentLineTransformers(model.ProjectFile));

					foreach (var textTransformer in languageServiceDocumentLineTransformers)
					{
						DocumentLineTransformers.Add(textTransformer);
					}
				}

				model.CodeAnalysisCompleted += (s, ee) =>
				{
                    Diagnostics = model.CodeAnalysisResults.Diagnostics;
                    
                    foreach(var marker in Diagnostics)
                    {
                        if (marker.Length == 0)
                        {
                            var line = TextDocument.GetLineByOffset(marker.StartOffset);
                            var endoffset = TextUtilities.GetNextCaretPosition(TextDocument, marker.StartOffset,
                                TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorderOrSymbol);

                            if (endoffset == -1)
                            {
                                marker.Length = line.Length;
                            }
                            else
                            {
                                marker.EndOffset = endoffset;
                            }
                        }
                    }

					HighlightingData =
						new ObservableCollection<SyntaxHighlightingData>(model.CodeAnalysisResults.SyntaxHighlightingData);

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

			intellisense = new IntellisenseViewModel(model, this);
			_completionHint = new CompletionHintViewModel();

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
			Dispatcher.UIThread.InvokeAsync(() => { ShellViewModel.Instance.StatusBar.InstanceCount--; });
			Model.ShutdownBackgroundWorkers();

			Console.WriteLine("Editor VM Destructed.");
		}

		#endregion

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
			get { return caretLocation; }
			set
			{
				this.RaiseAndSetIfChanged(ref caretLocation, value);

				if (!Intellisense.IsVisible)
				{
					Intellisense.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);
					CompletionHint.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);					
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
						return "Inconsolata";

					default:
						return "Inconsolata";
				}
			}
		}

		private CompletionHintViewModel _completionHint;

		public CompletionHintViewModel CompletionHint
		{
			get { return _completionHint; }
			set { this.RaiseAndSetIfChanged(ref _completionHint, value); }
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
			Dispatcher.UIThread.InvokeAsync(() => { CaretIndex = TextDocument.GetOffset(line, column); });
		}

		public void GotoOffset(int offset)
		{
			Dispatcher.UIThread.InvokeAsync(() => { CaretIndex = offset; });
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

		private int caretIndex;

		public int CaretIndex
		{
			get { return caretIndex; }
			set
			{
				if (TextDocument != null && value > TextDocument.TextLength)
				{
					value = TextDocument.TextLength - 1;
				}

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

				if (charClass != TextUtilities.CharacterClass.LineTerminator && prevChar != ' ' &&
					TextUtilities.GetCharacterClass(prevChar) != TextUtilities.CharacterClass.LineTerminator)
				{
					start = TextUtilities.GetNextCaretPosition(TextDocument, offset, TextUtilities.LogicalDirection.Backward,
						TextUtilities.CaretPositioningMode.WordStart);
				}

				var end = TextUtilities.GetNextCaretPosition(TextDocument, start, TextUtilities.LogicalDirection.Forward,
					TextUtilities.CaretPositioningMode.WordBorder);

				if (start != -1 && end != -1)
				{
					var word = TextDocument.GetText(start, end - start).Trim();

					if (TextUtilities.IsSymbol(word))
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

        public async Task<bool> UpdateToolTipAsync (int offset)
        {
            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor)
            {
                var matching = Model.CodeAnalysisResults?.Diagnostics.FindSegmentsContaining(offset).FirstOrDefault();

                if (matching != null)
                {
                    ToolTip = new ErrorProbeViewModel(matching);

                    return true;
                }
            }

            if (offset != -1 && ShellViewModel.Instance.CurrentPerspective == Perspective.Editor && Model.LanguageService != null)
            {
                var symbol = await Model.LanguageService.GetSymbolAsync(Model.ProjectFile, EditorModel.UnsavedFiles, offset);

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
                    var debugManager = IoC.Get<IDebugManager>();
                    
                    var evaluatedExpression = await debugManager.ProbeExpressionAsync(expression);

                    if (evaluatedExpression != null)
                    {
                        var newToolTip = new DebugHoverProbeViewModel(debugManager);
                        newToolTip.AddExistingWatch(evaluatedExpression);

                        ToolTip = newToolTip;
                        return true;
                    }
                }
            }

            return false;
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

		private TextSegmentCollection<Diagnostic> diagnostics;
		public TextSegmentCollection<Diagnostic> Diagnostics
		{
			get { return diagnostics; }
			set { this.RaiseAndSetIfChanged(ref diagnostics, value); }
		}

		public string Title
		{
			get
			{
				var result = Model.ProjectFile?.Name;

				if (Model.IsDirty)
				{
					result += '*';
				}

				return result;
			}
		}

		#endregion

		#region Commands

		public ReactiveCommand<object> BeforeTextChangedCommand { get; }
		public ReactiveCommand<object> TextChangedCommand { get; }
		public ReactiveCommand<object> SaveCommand { get; }
		public ReactiveCommand<object> CloseCommand { get; }

		// todo this menu item and command should be injected via debugging module.
		public ReactiveCommand<object> AddWatchCommand { get; }

		public ISourceFile ProjectFile
		{
			get { return Model.ProjectFile; }
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