using Avalonia.Input;
using AvaloniaEdit.Editing;
using System.Collections.Generic;

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
                _manager.Deactivate();
                e.Handled = true;
            }
        }
    }

    internal class RenameManager
    {
        private List<RenamingTextElement> _registeredElements;
        private RenameInputHandler _inputHandler;

        internal CodeEditor CodeEditor { get; }

        public RenameManager(CodeEditor editor)
        {
            CodeEditor = editor;
            _registeredElements = new List<RenamingTextElement>();
        }

        public RenamingTextElement RegisterElement(int start, int length)
        {
            var result = new RenamingTextElement(CodeEditor.TextArea, start, length);

            _registeredElements.Add(result);

            return result;
        }

        public void Activate (RenamingTextElement target)
        {
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

        public void Deactivate()
        {
            foreach(var element in _registeredElements)
            {
                element.Deactivate();
            }

            _registeredElements.Clear();

            CodeEditor.TextArea.PopStackedInputHandler(_inputHandler);
        }
    }
}
