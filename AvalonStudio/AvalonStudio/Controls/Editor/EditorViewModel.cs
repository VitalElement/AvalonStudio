namespace AvalonStudio.Controls
{
    using Models.Platform;
    using Models.LanguageServices;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using TextEditor.Document;
    using Perspex;
    using Perspex.Input;
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

            TextChangedCommand.Subscribe(_=>
            {
                Intellisense.IsVisible = true;
            });

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

                this.RaisePropertyChanged(nameof(TextDocument));
                this.RaisePropertyChanged(nameof(Title));
            };

            model.TextChanged += (sender, e) =>
            {
                this.RaisePropertyChanged(nameof(Title));
            };

            this.intellisense = new IntellisenseViewModel(model, this);
            intellisense.Add(new CodeCompletionData { Suggestion = "Test1" });
            intellisense.Add(new CodeCompletionData { Suggestion = "Test2" });
            intellisense.Add(new CodeCompletionData { Suggestion = "Test3" });
            intellisense.Add(new CodeCompletionData { Suggestion = "Test4" });
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


        private Point caretLocation;
        public Point CaretLocation
        {
            get { return caretLocation; }
            set
            {
                this.RaiseAndSetIfChanged(ref caretLocation, value);
                Intellisense.Position = new Thickness(caretLocation.X, caretLocation.Y, 0, 0);
            }
        }

        public string FontFamily
        {
            get
            {
                switch(Platform.PlatformID)
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
                Workspace.This.StatusBar.Offset = value;

                if (value >= 0)
                {
                    var location = TextDocument.GetLocation(value);
                    Workspace.This.StatusBar.LineNumber = location.Line;
                    Workspace.This.StatusBar.Column = location.Column;
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

                if(Model.IsDirty)
                {
                    result += '*';
                }

                return result;
            }
        }
        #endregion

        public void OnKeyDowm (KeyEventArgs e)
        {
            Intellisense.OnKeyDown(e);            
        }

        #region Commands
        public ReactiveCommand<object> BeforeTextChangedCommand { get; private set; }
        public ReactiveCommand<object> TextChangedCommand { get; private set; }
        public ReactiveCommand<object> SaveCommand { get; private set; }        
        #endregion

        #region Public Methods
        public void Save ()
        {
            Model.Save();

            this.RaisePropertyChanged(nameof(Title));
        }
        #endregion
    }
}
