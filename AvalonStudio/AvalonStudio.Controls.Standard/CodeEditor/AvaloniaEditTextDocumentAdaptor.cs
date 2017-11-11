using Avalonia.Input;
using AvaloniaEdit.Document;
using AvalonStudio.Extensibility.Documents;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class CodeEditorDocumentAdaptor : ITextDocument
    {
        private CodeEditor _codeEditor;

        public CodeEditorDocumentAdaptor(CodeEditor editor)
        {
            _codeEditor = editor;

            _codeEditor.TextArea.TextEntering += TextEntering;
            _codeEditor.TextArea.TextEntered += TextEntered;
        }

        ~CodeEditorDocumentAdaptor()
        {
            _codeEditor.TextArea.TextEntering -= TextEntering;
            _codeEditor.TextArea.TextEntered -= TextEntered;
        }

        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately before the TextArea handles the TextInput event.
        /// </summary>
        public event EventHandler<TextInputEventArgs> TextEntering;

        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately after the TextArea handles the TextInput event.
        /// </summary>
        public event EventHandler<TextInputEventArgs> TextEntered;


        public void Save()
        {
            _codeEditor.Save();
        }
    }
}
