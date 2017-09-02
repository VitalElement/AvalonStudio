namespace AvalonStudio.Languages.CPlusPlus
{
    using Avalonia.Input;
    using AvaloniaEdit.Document;
    using AvaloniaEdit.Rendering;
    using AvalonStudio.Extensibility.Editor;
    using AvalonStudio.Languages.CPlusPlus.Rendering;
    using NClang;
    using System;
    using System.Collections.Generic;

    internal class CPlusPlusDataAssociation
    {
        public ClangTranslationUnit TranslationUnit { get; set; }

        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}