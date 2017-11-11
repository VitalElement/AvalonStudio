using Avalonia.Input;
using AvalonStudio.Extensibility.Documents;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Editor
{
    public interface ICodeEditorInputHelper
    {
        void BeforeTextInput(ILanguageService languageService, ITextDocument document, TextInputEventArgs args);
        void AfterTextInput(ILanguageService languageServivce, ITextDocument document, TextInputEventArgs args);
    }
}
