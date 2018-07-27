using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Languages;

namespace AvalonStudio.Editor
{
    public interface ICodeEditorInputHelper
    {
        void BeforeTextInput(ILanguageService languageService, ITextEditor editor,  string inputText);
        void AfterTextInput(ILanguageService languageServivce, ITextEditor document, string inputText);
    }
}
