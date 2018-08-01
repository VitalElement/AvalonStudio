using AvalonStudio.Editor;
using System;
using System.Linq;
using System.Text;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    public class CIndentationTests
    {
        [Fact]
        public void DefaultIndentationCopiesPreviousLineIndentation()
        {
            string testData = @"    ";

            var testEditor = TestEditorManager.Create(testData);

            testEditor.SetCursor(4);
            testEditor.Input("\n");

            Assert.Equal("    \n    ", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_Inserts_Indentation_When_NewLine_Inserted_Between_2_Braces ()
        {
            string testData = @"{}";

            var testEditor = TestEditorManager.Create(testData, new CBasedLanguageIndentationInputHelper());

            testEditor.SetCursor(1);
            testEditor.Input("\n");

            Assert.Equal("{\n    \n}", testEditor.Document.Text);
        }
    }
}
