using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Editor
{
    public class DocumentLinesCollection : IIndexableList<IDocumentLine>
    {
        private AvaloniaEdit.Document.TextDocument _document;

        public DocumentLinesCollection(AvaloniaEdit.Document.TextDocument document)
        {
            _document = document;
        }

        public IDocumentLine this[int index] => new DocumentLine(_document.Lines[index - 1]);

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

    //public class EditorAdaptor : IEditor
    //{
    //    private CodeEditor _codeEditor;
    //    private AvalonStudioTextDocument _document;
    //    private ISourceFile _sourceFile;

    //    public EditorAdaptor(CodeEditor editor)
    //    {
    //        _codeEditor = editor;
    //        _document = new AvalonStudioTextDocument(editor.Document);

    //        _codeEditor.TextArea.TextEntering += _codeEditor_TextEntering;
    //        _codeEditor.TextArea.TextEntered += _codeEditor_TextEntered;
    //        _codeEditor.RequestTooltipContent += RequestTooltipContent;
    //        _codeEditor.LostFocus += _codeEditor_LostFocus;
    //    }

    //    private void _codeEditor_TextEntering (object sender, TextInputEventArgs e)
    //    {
    //        if (!_codeEditor.IsReadOnly)
    //        {
    //            TextEntering?.Invoke(this, e);
    //        }
    //    }

    //    private void _codeEditor_TextEntered (object sender, TextInputEventArgs e)
    //    {
    //        if (!_codeEditor.IsReadOnly)
    //        {
    //            TextEntered?.Invoke(this, e);
    //        }
    //    }

    //    private void _codeEditor_LostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    //    {
    //        LostFocus?.Invoke(this, EventArgs.Empty);
    //    }

    //    public ITextDocument Document => _document;

    //    public void Save()
    //    {
    //        _codeEditor.Save();
    //    }

    //    public void IndentLine(int line)
    //    {
    //       // _codeEditor.LanguageService?.IndentationStrategy.IndentLine(_codeEditor.Document, _codeEditor.Document.GetLineByNumber(line));
    //    }

    //    public int CaretOffset { get => _codeEditor.CaretOffset; set => _codeEditor.CaretOffset = value; }

    //    public int Line { get => _codeEditor.TextArea.Caret.Line; set => _codeEditor.TextArea.Caret.Line = value; }

    //    public int Column { get => _codeEditor.TextArea.Caret.Column; set => _codeEditor.TextArea.Caret.Column = value; }

    //    public ISourceFile SourceFile => _sourceFile;
        
    //    /// <summary>
    //    /// Occurs when the TextArea receives text input.
    //    /// but occurs immediately before the TextArea handles the TextInput event.
    //    /// </summary>
    //    public event EventHandler<TextInputEventArgs> TextEntering;

    //    /// <summary>
    //    /// Occurs when the TextArea receives text input.
    //    /// but occurs immediately after the TextArea handles the TextInput event.
    //    /// </summary>
    //    public event EventHandler<TextInputEventArgs> TextEntered;

    //    public event EventHandler<TooltipDataRequestEventArgs> RequestTooltipContent;
    //    public event EventHandler LostFocus;        

    //    public void Dispose()
    //    {
    //        if (_codeEditor != null)
    //        {
    //            _codeEditor.Close();
    //            _codeEditor.TextArea.TextEntering -= TextEntering;
    //            _codeEditor.TextArea.TextEntered -= TextEntered;
    //            _codeEditor.LostFocus -= _codeEditor_LostFocus;
    //            _codeEditor = null;
    //        }

    //        _document?.Dispose();
    //        _document = null;
    //    }

    //    public void FormatAll()
    //    {
    //        _codeEditor.FormatAll();
    //    }

    //    public void Focus()
    //    {
    //        _codeEditor.Focus();
    //    }

    //    public void TriggerCodeAnalysis()
    //    {
    //        _codeEditor.TriggerCodeAnalysis();
    //    }

    //    public void Comment()
    //    {
    //        _codeEditor.CommentSelection();
    //    }

    //    public void Uncomment()
    //    {
    //        _codeEditor.UncommentSelection();
    //    }

    //    public void Undo()
    //    {
    //        _codeEditor.Document.UndoStack.Undo();
    //    }

    //    public void Redo()
    //    {
    //        _codeEditor.Document.UndoStack.Redo();
    //    }

    //    public void SetDebugHighlight(int line, int startColumn, int endColumn)
    //    {
    //        //_codeEditor.SetDebugHighlight(line, startColumn, endColumn);
    //    }

    //    public void ClearDebugHighlight()
    //    {
    //        //_codeEditor.ClearDebugHighlight();
    //    }

    //    public void GotoOffset(int offset)
    //    {
    //        _codeEditor.CaretOffset = offset;
    //    }

    //    public void GotoPosition(int line, int column)
    //    {
    //        _codeEditor.CaretOffset = _codeEditor.Document.GetOffset(line, column);
    //    }

    //    public void RenameSymbol(int offset)
    //    {
    //        _codeEditor.BeginSymbolRename(offset);
    //    }

    //    internal CodeEditor EditorImpl => _codeEditor;
    //}

    public class AvalonStudioTextDocumentAnchoredSegment : ISegment
    {
        private AvaloniaEdit.Document.AnchorSegment _impl;

        public AvalonStudioTextDocumentAnchoredSegment (AvalonStudioTextDocument document, int start, int length)
        {
            _impl = new AvaloniaEdit.Document.AnchorSegment(document.Document, start, length);
        }

        public int Offset => _impl.Offset;

        public int Length => _impl.Length;

        public int EndOffset => _impl.EndOffset;
    }

    public class AvalonStudioTextDocument : ITextDocument, IDisposable
    {
        public static async Task<ITextDocument> CreateAsync (ISourceFile file)
        {
            return await CreateAsync(file.Location);
        }

        public static async Task<ITextDocument> CreateAsync (string path)
        {
            using (var fileStream = File.OpenText(path))
            {
                var text = await fileStream.ReadToEndAsync();

                var document = new AvaloniaEdit.Document.TextDocument(text);

                return new AvalonStudioTextDocument(document);
            }
        }

        public static ITextDocument Create (string text)
        {
            return new AvalonStudioTextDocument(new AvaloniaEdit.Document.TextDocument(text));
        }

        private AvaloniaEdit.Document.TextDocument _document;
        private readonly DocumentLinesCollection _lines;

        public event EventHandler<DocumentChangeEventArgs> Changed;

        internal AvaloniaEdit.Document.TextDocument Document => _document;

        public AvalonStudioTextDocument(AvaloniaEdit.Document.TextDocument document)
        {
            _document = document;
            _lines = new DocumentLinesCollection(document);
            _document.Changed += _document_Changed;
        }

        private void _document_Changed(object sender, AvaloniaEdit.Document.DocumentChangeEventArgs e)
        {
            Changed?.Invoke(this, new DocumentChangeEventArgs(e.Offset, e.RemovedText.Text, e.InsertedText.Text));
        }

        ~AvalonStudioTextDocument()
        {
        }

        public ISegment CreateAnchoredSegment (int offset, int length)
        {
            return new AvalonStudioTextDocumentAnchoredSegment(this, offset, length);
        }

        public string Text => _document.Text;

        public IIndexableList<IDocumentLine> Lines => _lines;

        public int LineCount => _document.LineCount;

        public int TextLength => _document.TextLength;

        public void Insert(int offset, string text)
        {
            Replace(offset, 0, text);
        }

        public void Replace(int offset, int length, string text, ReplaceMode replaceMode = ReplaceMode.Normal)
        {
            _document.Replace(offset, length, text, (AvaloniaEdit.Document.OffsetChangeMappingType)replaceMode);
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

        public IDocumentLine GetLineByNumber(int lineNumber) => Lines[lineNumber];

        public TextLocation GetLocation(int offset)
        {
            var loc = _document.GetLocation(offset);

            return new TextLocation(loc.Line, loc.Column);
        }

        public int GetOffset(int line, int column)
        {
            return _document.GetOffset(line, column);
        }

        public void Undo ()
        {
            _document.UndoStack.Undo();
        }

        public void Redo()
        {
            _document.UndoStack.Redo();
        }

        public void Dispose()
        {
            _document.Changed -= _document_Changed;
            _document = null;
        }
    }
}
