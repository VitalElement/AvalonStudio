using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Editor;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Tests
{
    /// <summary>
    /// Provides a simple wrapper around ITextEditor that can be used for testing editor and document manipulation.
    /// It essentially simulates the behavior of the full editor in terms of updating caret locations as text is manipulated.
    /// </summary>
    public class TestEditorManager
    {
        public static TestEditorManager Create (string initialText, params ITextEditorInputHelper[] helpers)
        {
            return new TestEditorManager(initialText, helpers);
        }

        private TestEditorManager(string initialText, IEnumerable<ITextEditorInputHelper> helpers = null)
        {
            Document = AvalonStudioTextDocument.Create(initialText);

            Editor = new TextEditorViewModel(Document, null);

            if (helpers != null)
            {
                foreach (var helper in helpers)
                {
                    (Editor as TextEditorViewModel).InputHelpers.Add(helper);
                }
            }

            Document.Changed += (sender, e) =>
            {
                if (e.Offset <= Editor.Offset)
                {
                    SetCursor(Editor.Offset + e.InsertionLength - e.RemovalLength);
                }
            };
        }

        public void Input (string text)
        {
            Editor.OnBeforeTextEntered(text);
            
            Document.Replace(Editor.Offset, 0, text);
            Editor.OnTextEntered(text);
        }

        public void SetCursor (int offset)
        {
            Editor.Offset = offset;

            var location = Editor.Document.GetLocation(Editor.Offset);

            Editor.Line = location.Line;
            Editor.Column = location.Column;
        }

        public ITextDocument Document { get; private set; }

        public ITextEditor Editor { get; private set; }
    }
}
