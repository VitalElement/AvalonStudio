using System.Diagnostics;

namespace AvalonStudio.TextEditor.Document
{
    /// <summary>
    ///     Describes a change to a TextDocument.
    /// </summary>
    internal sealed class DocumentChangeOperation : IUndoableOperationWithContext
    {
        private readonly DocumentChangeEventArgs change;
        private readonly TextDocument document;

        public DocumentChangeOperation(TextDocument document, DocumentChangeEventArgs change)
        {
            this.document = document;
            this.change = change;
        }

        public void Undo(UndoStack stack)
        {
            Debug.Assert(stack.state == UndoStack.StatePlayback);
            stack.RegisterAffectedDocument(document);
            stack.state = UndoStack.StatePlaybackModifyDocument;
            Undo();
            stack.state = UndoStack.StatePlayback;
        }

        public void Redo(UndoStack stack)
        {
            Debug.Assert(stack.state == UndoStack.StatePlayback);
            stack.RegisterAffectedDocument(document);
            stack.state = UndoStack.StatePlaybackModifyDocument;
            Redo();
            stack.state = UndoStack.StatePlayback;
        }

        public void Undo()
        {
            var map = change.OffsetChangeMapOrNull;
            document.Replace(change.Offset, change.InsertionLength, change.RemovedText, map != null ? map.Invert() : null);
        }

        public void Redo()
        {
            document.Replace(change.Offset, change.RemovalLength, change.InsertedText, change.OffsetChangeMapOrNull);
        }
    }
}