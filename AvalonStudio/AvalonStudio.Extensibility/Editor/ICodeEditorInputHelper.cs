using Avalonia.Input;
using AvalonStudio.Documents;
using AvalonStudio.Languages;

namespace AvalonStudio.Editor
{
    public interface ICodeEditorInputHelper
    {
        void BeforeTextInput(ILanguageService languageService, IEditor document, TextInputEventArgs args);
        void AfterTextInput(ILanguageService languageServivce, IEditor document, TextInputEventArgs args);
    }
}
