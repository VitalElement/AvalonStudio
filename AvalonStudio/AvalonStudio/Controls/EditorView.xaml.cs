using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls
{
    public class EditorView : UserControl
    {
        private readonly CompositeDisposable disposables;
        private AvaloniaEdit.TextEditor editor;
        private EditorViewModel editorViewModel;

        public EditorView()
        {
            InitializeComponent();

            disposables = new CompositeDisposable();
        }

        ~EditorView()
        {
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            editor = this.Find<AvaloniaEdit.TextEditor>("editor");

            //editor.CaretChangedByPointerClick += Editor_CaretChangedByPointerClick;
            //editor.EditorScrolled += Editor_EditorScrolled;

            editorViewModel = DataContext as EditorViewModel;

            if (editorViewModel != null && editor != null)
            {
                editorViewModel.Model.Editor = editor;
                editor.Focus();
            }
        }

        private void Editor_EditorScrolled(object sender, EventArgs e)
        {
            editorViewModel.Intellisense.IsVisible = false;
        }

        private void Editor_CaretChangedByPointerClick(object sender, EventArgs e)
        {
            editorViewModel.Intellisense.IsVisible = false;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
           // editor.EditorScrolled -= Editor_EditorScrolled;
            //editor.CaretChangedByPointerClick -= Editor_CaretChangedByPointerClick;

            editor = null;
            editorViewModel = null;

            disposables.Dispose();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}