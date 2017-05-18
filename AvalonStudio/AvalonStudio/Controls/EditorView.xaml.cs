using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.LogicalTree;

namespace AvalonStudio.Controls
{
    public class EditorView : UserControl, ICodeEditor
    {
        private readonly CompositeDisposable disposables;
        private EditorViewModel editorViewModel;
        private Standard.CodeEditor.CodeEditor _editor;

        public EditorView()
        {
            InitializeComponent();

            disposables = new CompositeDisposable();

            disposables.Add(this.GetObservable(DataContextProperty).OfType<EditorViewModel>().Subscribe(vm => vm.Editor = this));
        }

        ~EditorView()
        {
            disposables.Dispose();
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            _editor = this.FindControl<Standard.CodeEditor.CodeEditor>("editor");
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
            editorViewModel = null;

            disposables.Dispose();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void Close()
        {
            _editor.UnRegisterLanguageService();

            // TODO dispose disposables.
        }
    }
}