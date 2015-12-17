namespace AvalonStudio.Controls
{
    using Models.Platform;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TextEditor.Document;
    using Perspex;
    using Perspex.Input;
    using System.Windows.Input;
    using Languages;
    public class EditorViewModel : ViewModel<EditorModel>
    {
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

            tabCharacter = "    ";

            model.DocumentLoaded += (sender, e) =>
            {
                model.CodeAnalysisCompleted += (s, ee) =>
                {                    
                    Diagnostics = model.CodeAnalysisResults.Diagnostics;
                    HighlightingData = new ObservableCollection<SyntaxHighlightingData>(model.CodeAnalysisResults.SyntaxHighlightingData);
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
        }
        #endregion

        #region Properties
        private string tabCharacter;
        public string TabCharacter
        {
            get { return tabCharacter; }
            set { this.RaiseAndSetIfChanged(ref tabCharacter, value); }
        }

        private string wordAtCaret;
        public string WordAtCaret
        {
            get { return wordAtCaret; }
            set { this.RaiseAndSetIfChanged(ref wordAtCaret, value); }
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
                    Intellisense.Position = new Thickness(caretLocation.X, caretLocation.Y + LineHeight, 0, 0);
                }
            }
        }

        public string FontFamily
        {
            get
            {
                switch (Platform.PlatformID)
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

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref caretIndex, value);
                Workspace.Instance.StatusBar.Offset = value;

                if (value >= 0)
                {
                    var location = TextDocument.GetLocation(value);
                    Workspace.Instance.StatusBar.LineNumber = location.Line;
                    Workspace.Instance.StatusBar.Column = location.Column;
                }
            }
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
                string result = Model.ProjectFile?.Title;

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
