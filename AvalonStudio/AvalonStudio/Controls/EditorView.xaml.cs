using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.LogicalTree;
using AvalonStudio.Documents;
using AvalonStudio.Projects;
using System.Threading.Tasks;
using AvalonStudio.Languages;
using AvalonStudio.Shell;
using AvalonStudio.Extensibility;
using Avalonia.Input;

namespace AvalonStudio.Controls
{
    public class EditorView : UserControl, IEditor
    {
        private readonly CompositeDisposable disposables;

        private Standard.CodeEditor.CodeEditor _editor;

        private EditorViewModel _editorViewModel;

        private IShell shell;

        public ISourceFile ProjectFile => throw new NotImplementedException();

        public int CaretOffset => _editor.CaretOffset;

        public EditorView()
        {
            InitializeComponent();

            disposables = new CompositeDisposable();

            disposables.Add(this.GetObservable(DataContextProperty).OfType<EditorViewModel>().Subscribe(vm =>
            {
                vm.AttachEditor(this);
                _editorViewModel = vm;
            }));
        }

        ~EditorView()
        {
            disposables.Dispose();
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            _editor = this.FindControl<Standard.CodeEditor.CodeEditor>("editor");

            _editor.RequestTooltipContent += _editor_RequestTooltipContent;

            shell = IoC.Get<IShell>();

            _editor.GetObservable(AvalonStudio.Controls.Standard.CodeEditor.CodeEditor.IsDirtyProperty).Subscribe(dirty =>
            {
                if (dirty && DataContext is EditorViewModel editorVm)
                {
                    editorVm.Dock = Dock.Left;

                    // Selecting the document event though it already is, causes it to be removed from the temporary document cache.
                    editorVm.IsTemporary = false;
                    shell.SelectedDocument = editorVm;
                }
            });

            _editor.AddHandler(PointerWheelChangedEvent, (sender, ee) => { _editorViewModel?.OnPointerWheelChanged(ee); }, Avalonia.Interactivity.RoutingStrategies.Tunnel, handledEventsToo: true);
        }

        private void _editor_RequestTooltipContent(object sender, Standard.TooltipDataRequestEventArgs e)
        {
            if (DataContext != null)
            {
                var editorVm = DataContext as EditorViewModel;

                e.GetViewModelAsyncTask = editorVm.UpdateToolTipAsync;
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            // editor.EditorScrolled -= Editor_EditorScrolled;
            //editor.CaretChangedByPointerClick -= Editor_CaretChangedByPointerClick;

            disposables.Dispose();
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            _editor.Focus();
        }

        public void TriggerCodeAnalysis()
        {
            _editor.TriggerCodeAnalysis();
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
            _editor.CommentSelection();
        }

        public void UnComment()
        {
            _editor.UncommentSelection();
        }

        public void Undo()
        {
            _editor.Document.UndoStack.Undo();
        }

        public void Redo()
        {
            _editor.Document.UndoStack.Redo();
        }

        public void SetDebugHighlight(int line, int startColumn, int endColumn)
        {
            _editor.SetDebugHighlight(line, startColumn, endColumn);
        }

        public void ClearDebugHighlight()
        {
            _editor.ClearDebugHighlight();
        }

        public void GotoOffset(int offset)
        {
            _editor.CaretOffset = offset;
            _editor.Focus();
        }

        public void GotoPosition(int line, int column)
        {
            _editor.TextArea.Caret.Line = line;
            _editor.TextArea.Caret.Column = column;
        }

        public void FormatAll()
        {
            _editor.FormatAll();
        }

        public async Task<Symbol> GetSymbolAsync(int offset)
        {
            return await _editor.GetSymbolAsync(offset);
        }

        public string GetWordAtOffset(int offset)
        {
            return _editor.GetWordAtOffset(offset);
        }
    }
}