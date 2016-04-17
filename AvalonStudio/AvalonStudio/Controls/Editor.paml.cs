namespace AvalonStudio.Controls
{
    using AvalonStudio.TextEditor;
    using Perspex;
    using Perspex.Controls;

    public class Editor : UserControl
    {
        private TextEditor editor;
        private EditorViewModel editorViewModel;

        public Editor()
        {
            InitializeComponent();

            editor = this.Find<TextEditor>("editor");

            DataContextChanged += (sender, e) =>
            {
                if (editorViewModel != DataContext)
                {
                    editorViewModel = DataContext as EditorViewModel;

                    if (editorViewModel != null && editor != null)
                    {
                        editorViewModel.Model.Editor = editor;
                        editor.Focus();
                    }
                }
            };
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();

        }
    }
}
