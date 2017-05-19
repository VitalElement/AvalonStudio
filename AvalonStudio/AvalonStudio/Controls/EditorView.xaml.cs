using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.LogicalTree;
using AvalonStudio.Documents;
using AvalonStudio.Projects;

namespace AvalonStudio.Controls
{
    public class EditorView : UserControl, IEditor
    {
        private readonly CompositeDisposable disposables;
        private EditorViewModel editorViewModel;
        private Standard.CodeEditor.CodeEditor _editor;

        public ISourceFile ProjectFile => throw new NotImplementedException();

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

        public void Save()
        {
            _editor.Save();
        }

        public void Comment()
        {
            throw new NotImplementedException();
        }

        public void UnComment()
        {
            throw new NotImplementedException();
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }

        public void Redo()
        {
            throw new NotImplementedException();
        }

        public void SetDebugHighlight(int line, int startColumn, int endColumn)
        {
            
        }

        public void ClearDebugHighlight()
        {
            
        }

        public void GotoOffset(int offset)
        {
            throw new NotImplementedException();
        }
    }
}