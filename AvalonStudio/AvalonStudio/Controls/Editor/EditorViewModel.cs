namespace AvalonStudio.Controls
{
    using Perspex.MVVM;
    using System.IO;

    public class EditorViewModel : ViewModelBase
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
            set { text = value; OnPropertyChanged(); }
        }

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set { caretIndex = value; OnPropertyChanged(); Workspace.This.StatusBar.Offset = value; }
        }

    }
}
