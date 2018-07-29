using AvalonStudio.Documents;
using AvalonStudio.Utils;

namespace AvalonStudio.Editor
{
    public class AutoBrackedInputHelper : ITextEditorInputHelper
    {
        public bool AfterTextInput(ITextEditor editor, string inputText)
        {
            if (inputText.Length == 1)
            {
                var currentChar = inputText[0];

                if (currentChar.IsCloseBracketChar() && editor.Offset < editor.Document.TextLength && editor.Document.GetCharAt(editor.Offset) == currentChar)
                {
                    editor.Document.Replace(editor.Offset, 1, "");
                }
                else if (currentChar.IsOpenBracketChar())
                {
                    var closeChar = inputText[0].GetCloseBracketChar();

                    editor.Document.Insert(editor.Offset, closeChar.ToString());
                    editor.Offset--;
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
