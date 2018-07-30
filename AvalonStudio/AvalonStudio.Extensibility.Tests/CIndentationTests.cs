using AvalonStudio.Controls.Standard.CodeEditor;
using AvalonStudio.Documents;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Editor;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    public class CIndentationTests
    {
        [Fact]
        public void Test ()
        {
            string testData = @"{}";

            var document = AvalonStudioTextDocument.Create(testData);

            var editor = new TextEditorViewModel(document, null);

            var helper = new CBasedLanguageIndentationInputHelper();

            MoveCursor(editor, 2);

            RunInput(editor, helper, "\n");
        }

        private void RunInput(ITextEditor editor, ITextEditorInputHelper helper, string text)
        {
            helper.BeforeTextInput(editor, text);
            InputText(editor, text);
            helper.AfterTextInput(editor, text);
        }

        private void InputText (ITextEditor editor, string text)
        {
            editor.Document.Replace(editor.Offset, 0, text);

            MoveCursor(editor, editor.Offset + text.Length);
        }

        private void MoveCursor (ITextEditor editor, int offset)
        {
            editor.Offset = offset;

            var location = editor.Document.GetLocation(editor.Offset);

            editor.Line = location.Line;
            editor.Column = location.Column;
        }
    }
}
