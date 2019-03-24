using AvalonStudio.Controls.Editor;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Editor;
using System.Collections.Generic;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    /// <summary>
    /// Provides a simple wrapper around ITextEditor that can be used for testing editor and document manipulation.
    /// It essentially simulates the behavior of the full editor in terms of updating caret locations as text is manipulated.
    /// </summary>
    public class TestEditorManager
    {
        public void AssertEditorState (string expectedTextState)
        {
            var index = expectedTextState.IndexOf('|');

            if(index >= 0)
            {
                expectedTextState = expectedTextState.Replace("|", "");
                Assert.Equal(index, Editor.Offset);
            }

            Assert.Equal(expectedTextState, Document.Text);
        }

        /// <summary>
        /// Creates an instance of TestEditorManager
        /// </summary>
        /// <param name="initialText">The initial text of the document. If this contains a '|' charactor, it will be removed and the editor caret position set to that location.</param>
        /// <param name="helpers">Any helpers to use for testing.</param>
        /// <returns></returns>
        public static TestEditorManager Create (string initialText, params ITextEditorInputHelper[] helpers)
        {
            var index = initialText.IndexOf('|');

            if(index >= 0)
            {
                initialText = initialText.Replace("|", "");
            }

            var result = new TestEditorManager(initialText, helpers);

            if(index >= 0)
            {
                result.SetCursor(index);
            }

            return result;
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
            (Editor as TextEditorViewModel).SetCursorQuiet(offset);
        }

        public ITextDocument Document { get; private set; }

        public ITextEditor Editor { get; private set; }
    }
}
