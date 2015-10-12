namespace AvalonStudio.Controls
{
    using TextEditor;
    using ReactiveUI;
    using System.Collections.ObjectModel;
    using Perspex.Media;

    public class EditorViewModel : ReactiveObject
    {
        public EditorViewModel()
        {
            this.highlightingData = new ObservableCollection<SyntaxHighlightingData>();

            this.highlightingData.Add(new SyntaxHighlightingData() { Foreground = Brushes.Red, Start = 20, Length = 100 });
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref text, value); }
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




    }
}
