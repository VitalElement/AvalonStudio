using AvalonStudio.Documents;
using AvalonStudio.Editor;

namespace AvalonStudio.Extensibility.Editor
{
    /// <summary>
    /// Indentation helper that copies the indentation from the previous line.
    /// </summary>
    public class DefaultIndentationInputHelper : ITextEditorInputHelper
    {
        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            if (inputText == "\n")
            {
                var line = editor.CurrentLine();
                var previousLine = line.PreviousLine;
                if (previousLine != null)
                {
                    var indentationSegment = editor.Document.GetWhitespaceAfter(previousLine.Offset);
                    var indentation = editor.Document.GetText(indentationSegment);
                    // copy indentation to line
                    indentationSegment = editor.Document.GetWhitespaceAfter(line.Offset);
                    editor.Document.Replace(indentationSegment.Offset, indentationSegment.Length, indentation, ReplaceMode.RemoveAndInsert);
                    // RemoveAndInsert guarantees the caret moves behind the new indentation.
                }
            }

            return false;
        }

        public bool BeforeTextInput(ITextEditor editor, string inputText)
        {
            return false;
        }

        public void CaretMovedToEmptyLine(ITextEditor editor)
        {

        }
    }
}
