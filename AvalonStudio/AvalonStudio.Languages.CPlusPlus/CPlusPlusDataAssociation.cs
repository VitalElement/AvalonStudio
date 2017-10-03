namespace AvalonStudio.Languages.CPlusPlus
{
    using Avalonia.Input;
    using NClang;
    using System;

    internal class CPlusPlusDataAssociation
    {
        public ClangTranslationUnit TranslationUnit { get; set; }

        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
        public EventHandler<TextInputEventArgs> BeforeTextInputHandler { get; set; }
    }
}