using Avalonia.Input;
using AvalonStudio.Extensibility.Documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class DocumentLinesCollection : IIndexableList<IDocumentLine>
    {
        private AvaloniaEdit.Document.TextDocument _document;

        public DocumentLinesCollection(AvaloniaEdit.Document.TextDocument document)
        {
            _document = document;
        }

        public IDocumentLine this[int index] => new DocumentLine(_document.Lines[index]);

        public int Count => _document.Lines.Count;

        public IEnumerator<IDocumentLine> GetEnumerator()
        {
            return _document.Lines.Select(l => new DocumentLine(l)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _document.Lines.Select(l => new DocumentLine(l)).GetEnumerator();
        }
    }

    public class DocumentLine : IDocumentLine
    {
        AvaloniaEdit.Document.DocumentLine _line;

        public DocumentLine(AvaloniaEdit.Document.DocumentLine line)
        {
            _line = line;
        }

        public int TotalLength => _line.TotalLength;

        public int DelimiterLength => _line.DelimiterLength;

        public int LineNumber => _line.LineNumber;

        public IDocumentLine PreviousLine
        {
            get
            {
                if (_line.PreviousLine != null)
                {
                    return new DocumentLine(_line.PreviousLine);
                }

                return null;
            }
        }

        public IDocumentLine NextLine
        {
            get
            {
                if (_line.NextLine != null)
                {
                    return new DocumentLine(_line.NextLine);
                }

                return null;
            }
        }

        public int Offset => _line.Offset;

        public int Length => _line.Length;

        public int EndOffset => _line.EndOffset;
    }

    public class EditorAdaptor : IEditor
    {
        private CodeEditor _codeEditor;
        private DocumentAdaptor _document;

        public EditorAdaptor(CodeEditor editor)
        {
            _codeEditor = editor;
            _document = new DocumentAdaptor(editor.Document);

            _codeEditor.TextArea.TextEntering += TextEntering;
            _codeEditor.TextArea.TextEntered += TextEntered;
        }

        public ITextDocument Document => _document;

        public void Save()
        {
            _codeEditor.Save();
        }

        public void IndentLine(int line)
        {
            _codeEditor.LanguageService?.IndentationStrategy.IndentLine(_codeEditor.Document, _codeEditor.Document.GetLineByNumber(line));
        }

        public int Offset { get => _codeEditor.CaretOffset; set => _codeEditor.CaretOffset = value; }

        public int Line { get => _codeEditor.TextArea.Caret.Line; set => _codeEditor.TextArea.Caret.Line = value; }

        public int Column { get => _codeEditor.TextArea.Caret.Column; set => _codeEditor.TextArea.Caret.Column = value; }

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

        public void Dispose()
        {
            _codeEditor.TextArea.TextEntering -= TextEntering;
            _codeEditor.TextArea.TextEntered -= TextEntered;
            _codeEditor = null;
        }
    }

    public class DocumentAdaptor : ITextDocument
    {
        private AvaloniaEdit.Document.TextDocument _document;
        private DocumentLinesCollection _lines;

        internal AvaloniaEdit.Document.TextDocument Document => _document;

        public DocumentAdaptor(AvaloniaEdit.Document.TextDocument document)
        {
            _document = document;
            _lines = new DocumentLinesCollection(document);
        }

        ~DocumentAdaptor()
        {
            System.Console.WriteLine("Adaptor Disposed");
        }

        public string Text => _document.Text;

        public IIndexableList<IDocumentLine> Lines => _lines;

        public int LineCount => _document.LineCount;

        public int TextLength => _document.TextLength;

        public void Insert(int offset, string text)
        {
            Replace(offset, 0, text);
        }

        public void Replace(int offset, int length, string text)
        {
            _document.Replace(offset, length, text);
        }

        public char GetCharAt(int offset)
        {
            return _document.GetCharAt(offset);
        }

        public string GetText(int offset, int length)
        {
            return _document.GetText(offset, length);
        }

        public IDisposable RunUpdate()
        {
            return _document.RunUpdate();
        }

        public IDocumentLine GetLineByNumber(int lineNumber) => Lines[lineNumber - 1];
    }
}
