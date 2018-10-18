using AvalonStudio.Documents;

namespace AvalonStudio.Editor
{
    public interface ITextEditorInputHelper
    {
        bool BeforeTextInput(ITextEditor editor,  string inputText);
        bool AfterTextInput(ITextEditor editor, string inputText);
        void CaretMovedToEmptyLine(ITextEditor editor);
    }
}
