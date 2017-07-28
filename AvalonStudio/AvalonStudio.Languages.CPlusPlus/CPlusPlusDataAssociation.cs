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
    using System.Reactive.Subjects;

    internal class CPlusPlusDataAssociation
    {
        public CPlusPlusDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IVisualLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());

            DocumentLineTransformers.Add(TextColorizer);
            DocumentLineTransformers.Add(new DefineTextLineTransformer());
            DocumentLineTransformers.Add(new PragmaMarkTextLineTransformer());
            DocumentLineTransformers.Add(new IncludeTextLineTransformer());
            Diagnostics = new Subject<TextSegmentCollection<Diagnostic>>();
        }

        public Subject<TextSegmentCollection<Diagnostic>> Diagnostics { get; set; }
        public ClangTranslationUnit TranslationUnit { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IVisualLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}