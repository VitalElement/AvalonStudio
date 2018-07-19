using Avalonia.Input;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Languages;
using System.Collections.Generic;
using System.IO;

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
                _manager.AcceptRename(_manager.Deactivate(false));
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

        public void Start (IEnumerable<SymbolRenameInfo> renameLocations, int offset)
        {
            _renameLocations = renameLocations;
            RenamingTextElement masterElement = null;

            foreach (var location in renameLocations)
            {
                CodeEditor editor = null;

                if (CodeEditor.SourceFile.CompareTo(location.FileName) == 0)
                {
                    editor = CodeEditor;
                }
                else
                {
                    var currentTab = IoC.Get<IStudio>().GetDocument(location.FileName);

                    if(currentTab is EditorAdaptor adaptor)
                    {
                        editor = adaptor.EditorImpl;
                    }
                }
                
                if(editor != null)
                { 
                    foreach (var change in location.Changes)
                    {
                        var start = editor.Document.GetOffset(change.StartLine, change.StartColumn);
                        var end = editor.Document.GetOffset(change.EndLine, change.EndColumn);

                        var currentElement = RegisterElement(editor, start, end - start);

                        if (editor == CodeEditor && masterElement == null && offset >= start && offset <= end)
                        {
                            masterElement = currentElement;
                        }
                    }
                }
            }

            if (masterElement != null)
            {
                Activate(masterElement);
            }
            else
            {

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
            }
            else
            {
                Deactivate();
            }
        }

        public void AcceptRename(string text)
        {
            foreach(var location in _renameLocations)
            {
                if (CodeEditor.SourceFile.CompareTo(location.FileName) != 0)
                {
                    var currentTab = IoC.Get<IStudio>().GetDocument(location.FileName);

                    if(currentTab == null) //then the file wasnt already opened and changes havent been applied yet.
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

                        if(document != null)
                        {
                            var segments = new List<AnchorSegment>();

                            foreach(var change in location.Changes)
                            {
                                var start = document.GetOffset(change.StartLine, change.StartColumn);
                                var end = document.GetOffset(change.EndLine, change.EndColumn);

                                segments.Add(new AnchorSegment(document, start, end - start));    
                            }
                            
                            foreach(var segment in segments)
                            {
                                document.Replace(segment, text);
                            }

                            File.WriteAllText(location.FileName, document.Text);
                        }
                        else
                        {
                            throw new System.Exception("Error renaming symbol");
                        }                        
                    }
                }
            }

            _renameLocations = null;
        }

        public string Deactivate(bool reset = true)
        {
            string result = "";

            if (reset)
            {
                _target.Text = _originalText;                
                _renameLocations = null;
            }
            else
            {
                result = _target.Text;
            }

            _target = null;

            foreach (var element in _registeredElements)
            {
                element.Deactivate();
            }

            _registeredElements.Clear();            

            CodeEditor.TextArea.PopStackedInputHandler(_inputHandler);

            return result;
        }
    }
}
