namespace AvalonStudio.Controls
{
    using AvalonStudio.TextEditor;
    using Perspex;
    using Perspex.Controls;
    using System.Reactive.Disposables;
    using System;

    public class Editor : UserControl
    {
        private TextEditor editor;
        private EditorViewModel editorViewModel;
        private CompositeDisposable disposables;

        ~Editor()
        {
            System.Console.WriteLine(("Editor UserControl Destructed."));
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            editor = this.Find<TextEditor>("editor");
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            editor = null;
            editorViewModel = null;

            disposables.Dispose();
        }

        public Editor()
        {
            disposables = new CompositeDisposable();
            InitializeComponent();

            disposables.Add(DataContextProperty.Changed.Subscribe((o) =>
            {
                if (o.OldValue is EditorViewModel)
                {
                    (o.OldValue as EditorViewModel).Model.Editor = null;
                }

                if (editorViewModel != DataContext)
                {
                    editorViewModel = DataContext as EditorViewModel;

                    if (editorViewModel != null && editor != null)
                    {
                        editorViewModel.Model.Editor = editor;
                        editor.Focus();
                    }
                }
            }));
        }


        private void InitializeComponent()
        {
            this.LoadFromXaml();

        }
    }
}
