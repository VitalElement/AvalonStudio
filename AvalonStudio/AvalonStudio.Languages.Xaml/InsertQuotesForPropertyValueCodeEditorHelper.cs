﻿using Avalonia.Ide.CompletionEngine;
using Avalonia.Input;
using AvalonStudio.Editor;
using AvalonStudio.Extensibility.Documents;
using System;

namespace AvalonStudio.Languages.Xaml
{
    class InsertQuotesForPropertyValueCodeEditorHelper : ICodeEditorInputHelper
    {
        public void AfterTextInput(ILanguageService languageServivce, IEditor editor, TextInputEventArgs args)
        {
            if (args.Text == "=")
            {
                var textBefore = editor.Document.GetText(0, Math.Max(0, editor.Offset - 1));

                if (textBefore.Length > 2 && textBefore[textBefore.Length - 1] != '/')
                {
                    var state = XmlParser.Parse(textBefore);
                    if (state.State == XmlParser.ParserState.StartAttribute)
                    {
                        var caret = editor.Offset;
                        editor.Document.Replace(caret, 0, "\"\"");
                        editor.Offset = caret + 1;
                    }
                }
            }
        }

        public void BeforeTextInput(ILanguageService languageService, IEditor editor, TextInputEventArgs args)
        {
        }
    }
}