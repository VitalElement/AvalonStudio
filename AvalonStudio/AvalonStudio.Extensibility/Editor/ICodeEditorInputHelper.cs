using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Languages;

namespace AvalonStudio.Editor
{
    public interface ICodeEditorInputHelper
    {
        bool BeforeTextInput(ILanguageService languageService, ITextEditor editor,  string inputText);
        bool AfterTextInput(ILanguageService languageServivce, ITextEditor editor, string inputText);
    }
}
