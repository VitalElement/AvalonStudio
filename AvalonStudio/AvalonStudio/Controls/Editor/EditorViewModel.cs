namespace AvalonStudio.Controls
{
    using ReactiveUI;
    using System.IO;

    public class EditorViewModel : ReactiveObject
    {
        public EditorViewModel()
        {
            var fs = File.Open("CardLaminator.cpp", FileMode.Open);

            StreamReader sr = new StreamReader(fs);

            Text = sr.ReadToEnd();

            sr.Close();
            fs.Close();
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

    }
}
