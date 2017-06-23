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
        public CPlusPlusDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IVisualLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);
            DocumentLineTransformers.Add(new DefineTextLineTransformer());
            DocumentLineTransformers.Add(new PragmaMarkTextLineTransformer());
            DocumentLineTransformers.Add(new IncludeTextLineTransformer());
        }

        public ClangTranslationUnit TranslationUnit { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IVisualLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}