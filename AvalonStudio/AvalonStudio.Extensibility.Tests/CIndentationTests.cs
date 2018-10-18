using AvalonStudio.Editor;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    public class CIndentationTests
    {
        [Fact]
        public void DefaultIndentationCopiesPreviousLineIndentation()
        {
            var testEditor = TestEditorManager.Create("    |");

            testEditor.Input("\n");

            Assert.Equal("    \n    ", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_Inserts_Indentation_When_NewLine_Inserted_Between_2_Braces ()
        {
            var testEditor = TestEditorManager.Create("{|}", new CBasedLanguageIndentationInputHelper());

            testEditor.Input("\n");

            Assert.Equal("{\n    \n}", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_ReIndents_Statement_With_Too_little_Indentation_On_SemiColon()
        {
            var testEditor = TestEditorManager.Create("{\nstatement()|\n}", new CBasedLanguageIndentationInputHelper());

            testEditor.Input(";");

            Assert.Equal("{\n    statement();\n}", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_ReIndents_Statement_With_Too_Much_Indentation_On_SemiColon()
        {
            var testEditor = TestEditorManager.Create("{\n        statement()|\n}", new CBasedLanguageIndentationInputHelper());

            testEditor.Input(";");

            Assert.Equal("{\n    statement();\n}", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_ReIndents_Line_Following_Conditional_Statement()
        {
            var testEditor = TestEditorManager.Create("{\n    if(statement)|\n}", new CBasedLanguageIndentationInputHelper());

            testEditor.Input("\n");

            Assert.Equal("{\n    if(statement)\n        \n}", testEditor.Document.Text);
        }

        [Fact]
        public void C_Indentation_ReIndents_Line_Following_Else_Statement()
        {
            var testEditor = TestEditorManager.Create("{\n    if(statement)\n        statement;\n    else|\n}", new CBasedLanguageIndentationInputHelper());

            testEditor.Input("\n");

            Assert.Equal("{\n    if(statement)\n        statement;\n    else\n        \n}", testEditor.Document.Text);
        }
    }
}
