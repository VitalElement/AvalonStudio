namespace AvalonStudio.Controls
{
    using TextEditor.Document;
    using MVVM;
    using Perspex.Media;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using TextEditor;

    public class EditorViewModel : ViewModel<EditorModel>
    {
        public EditorViewModel(EditorModel model) : base(model)
        {
            this.highlightingData = new ObservableCollection<SyntaxHighlightingData>();

            BeforeTextChangedCommand = ReactiveCommand.Create();
            BeforeTextChangedCommand.Subscribe(model.OnBeforeTextChanged);

            TextChangedCommand = ReactiveCommand.Create();
            TextChangedCommand.Subscribe(model.OnTextChanged);

            model.DocumentLoaded += (sender, e) =>
            {
                model.Document.CodeAnalysisDataChanged += (s, ee) =>
                {
                    HighlightingData = new ObservableCollection<SyntaxHighlightingData>(model.Document.SyntaxHighlightingData);                                        
                };

                this.RaisePropertyChanged(() => TextDocument);
            };
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
            set { this.RaiseAndSetIfChanged(ref caretIndex, value); Workspace.This.StatusBar.Offset = value; }
        }

        private ObservableCollection<SyntaxHighlightingData> highlightingData;
        public ObservableCollection<SyntaxHighlightingData> HighlightingData
        {
            get { return highlightingData; }
            set { this.RaiseAndSetIfChanged(ref highlightingData, value); }
        }

        public ReactiveCommand<object> BeforeTextChangedCommand { get; private set; }
        public ReactiveCommand<object> TextChangedCommand { get; private set; }
    }
}
