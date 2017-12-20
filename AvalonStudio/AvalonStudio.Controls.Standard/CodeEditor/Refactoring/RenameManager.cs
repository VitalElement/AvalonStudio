using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Shell;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.CodeEditor.Refactoring
{
    internal class RenameInputHandler : TextAreaStackedInputHandler
    {
        private RenameManager _manager;

        public RenameInputHandler(RenameManager manager) : base(manager.CodeEditor.TextArea)
        {
            _manager = manager;
        }

        public override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Escape)
            {
                _manager.Deactivate();
                e.Handled = true;
            }
            else if (e.Key == Key.Return)
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await _manager.AcceptRename();
                });
                
                e.Handled = true;
            }
        }
    }

    internal class RenameManager
    {
        private List<RenamingTextElement> _registeredElements;
        private RenameInputHandler _inputHandler;
        private RenamingTextElement _target;
        private string _originalText;
        private IEnumerable<SymbolRenameInfo> _renameLocations;

        internal CodeEditor CodeEditor { get; }

        public RenameManager(CodeEditor editor)
        {
            CodeEditor = editor;
            _registeredElements = new List<RenamingTextElement>();            
        }

        private RenamingTextElement RegisterElement(CodeEditor editor, int start, int length)
        {
            var result = new RenamingTextElement(editor.TextArea, start, length);

            _registeredElements.Add(result);

            return result;
        }

        private void Clear()
        {
            foreach(var element in _registeredElements)
            {
                element.Deactivate();
            }

            _registeredElements.Clear();
        }

        private object _currentContext;
        private async Task<RenamingTextElement> ApplyRenameResults (int offset, bool initialise)
        {
            var (renameContext, renameInfo) = await CodeEditor.LanguageService.RenameSymbol
                (CodeEditor.DocumentAccessor, CodeEditor.CaretOffset, initialise ? "" : _target.Text, initialise ? null : _currentContext);

            _currentContext = renameContext;

            if (initialise)
            {
                Clear();
            }

            RenamingTextElement masterElement = null;

            foreach (var location in renameInfo)
            {
                CodeEditor editor = null;

                if (CodeEditor.SourceFile.CompareTo(location.FileName) == 0)
                {
                    editor = CodeEditor;
                }
                else
                {
                    var currentTab = IoC.Get<IShell>().GetDocument(location.FileName);

                    if (currentTab is EditorAdaptor adaptor)
                    {
                        editor = adaptor.EditorImpl;
                    }
                }

                if (editor != null)
                {                    
                    if (initialise)
                    {
                        foreach (var change in location.Changes)
                        {
                            var start = editor.Document.GetOffset(change.StartLine, change.StartColumn);
                            var end = editor.Document.GetOffset(change.EndLine, change.EndColumn);

                            var currentElement = RegisterElement(editor, start, end - start);

                            if (initialise && editor == CodeEditor && masterElement == null && offset >= start && offset <= end)
                            {
                                masterElement = currentElement;
                            }
                        }
                    }
                    else
                    {
                        inUpdate = true;
                        editor.Document.BeginUpdate();

                        foreach (var change in location.Changes)
                        {                            
                            var start = editor.Document.GetOffset(change.StartLine, change.StartColumn);
                            var end = editor.Document.GetOffset(change.EndLine, change.EndColumn);

                            if (!_target.Segment.Contains(start, end - start))
                            {
                                editor.Document.Replace(start, end - start, change.NewText);
                            }
                        }
                        
                        editor.Document.EndUpdate();
                        inUpdate = false;
                    }
                }
            }

            if (masterElement != null)
            {
                Activate(masterElement);
            }

            return masterElement;
        }

        public void Start(int offset)
        {
            if (CodeEditor.LanguageService != null)
            {
                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                    await ApplyRenameResults(offset, true);
                });
            }
        }

        public void Activate(RenamingTextElement target)
        {
            _target = target;
            _originalText = target.Text;            

            if (_registeredElements.Count > 0)
            {
                foreach (var element in _registeredElements)
                {
                    element.Activate(target);
                }

                _inputHandler = new RenameInputHandler(this);

                // disable existing snippet input handlers - there can be only 1 active snippet
                foreach (TextAreaStackedInputHandler h in CodeEditor.TextArea.StackedInputHandlers)
                {
                    if (h is RenameInputHandler)
                        CodeEditor.TextArea.PopStackedInputHandler(h);
                }
                CodeEditor.TextArea.PushStackedInputHandler(_inputHandler);
                
                _target.TextChanged += _target_TextChanged;
            }
            else
            {
                Deactivate();
            }
        }

        bool inUpdate = false;
        private void _target_TextChanged(object sender, System.EventArgs e)
        {
            if (!inUpdate)
            {
                inUpdate = true;
                CodeEditor.Undo();                

                Dispatcher.UIThread.InvokeAsync(async () =>
                {
                  await ApplyRenameResults(CodeEditor.CaretOffset, false);




                //foreach (var element in _registeredElements.Where(element => element != _target))
                //{
                //    element.UpdateTextToTarget();
                //}
            });
            }
        }

        public async Task AcceptRename()
        {   
            foreach (var location in _renameLocations)
            {
                if (CodeEditor.SourceFile.CompareTo(location.FileName) != 0)
                {
                    var currentTab = IoC.Get<IShell>().GetDocument(location.FileName);

                    if (currentTab == null) //then the file wasnt already opened and changes havent been applied yet.
                    {
                        TextDocument document = null;

                        using (var fs = new FileStream(location.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete, bufferSize: 4096, useAsync: false))
                        {
                            using (var reader = new StreamReader(fs))
                            {
                                document = new TextDocument(reader.ReadToEnd())
                                {
                                    FileName = location.FileName
                                };
                            }
                        }

                        if (document != null)
                        {
                            var segments = new List<AnchorSegment>();

                            foreach (var change in location.Changes)
                            {
                                var start = document.GetOffset(change.StartLine, change.StartColumn);
                                var end = document.GetOffset(change.EndLine, change.EndColumn);

                                segments.Add(new AnchorSegment(document, start, end - start));
                            }

                            foreach (var segment in segments)
                            {
                                document.Replace(segment, _target.Text);
                            }

                            File.WriteAllText(location.FileName, document.Text);
                            //Todo notify language service that this file has changed.
                        }
                        else
                        {
                            throw new System.Exception("Error renaming symbol");
                        }
                    }
                }
            }

            Deactivate(false);

            _renameLocations = null;
        }

        public void Deactivate(bool reset = true)
        {
            if (reset)
            {
                _target.Text = _originalText;
                _renameLocations = null;
            }
            
            _target.TextChanged -= _target_TextChanged;            
            _target = null;

            foreach (var element in _registeredElements)
            {
                element.Deactivate();
            }

            _registeredElements.Clear();

            CodeEditor.TextArea.PopStackedInputHandler(_inputHandler);            
        }
    }
}
