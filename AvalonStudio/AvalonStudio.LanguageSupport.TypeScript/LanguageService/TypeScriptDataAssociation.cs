using Avalonia.Input;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;

namespace AvalonStudio.LanguageSupport.TypeScript.LanguageService
{
    internal class TypeScriptDataAssociation
    {
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}