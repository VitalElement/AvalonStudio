namespace AvalonStudio.Controls
{
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

            this.highlightingData.Add(new SyntaxHighlightingData() { Foreground = Brushes.Red, Start = 20, Length = 100 });

            BeforeTextChangedCommand = ReactiveCommand.Create();
            BeforeTextChangedCommand.Subscribe(model.OnBeforeTextChanged);

            TextChangedCommand = ReactiveCommand.Create();
            TextChangedCommand.Subscribe(model.OnTextChanged);
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { Model.Text = value; this.RaiseAndSetIfChanged(ref text, value); }
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
