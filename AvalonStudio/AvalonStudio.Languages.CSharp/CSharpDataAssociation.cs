namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using AvalonStudio.Languages;
    using CPlusPlus;
    using Projects.OmniSharp;
    using System;
    using System.Collections.Generic;
    using TextEditor.Document;
    using TextEditor.Rendering;
    internal class CSharpDataAssociation
    {
        public CSharpDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);
        }

        public OmniSharpSolution Solution { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
    }
}