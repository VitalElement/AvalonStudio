namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using AvaloniaEdit.Document;
    using AvaloniaEdit.Rendering;
    using AvalonStudio.Extensibility.Editor;
    using AvalonStudio.Languages;
    using CPlusPlus;
    using Projects.OmniSharp;
    using System;
    using System.Collections.Generic;
    using System.Reactive.Subjects;

    internal class CSharpDataAssociation
    {
        public CSharpDataAssociation(AvaloniaEdit.Document.TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IVisualLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);

            Diagnostics = new Subject<TextSegmentCollection<Languages.Diagnostic>>();
        }

        public Subject<TextSegmentCollection<Diagnostic>> Diagnostics { get; set; }

        public OmniSharpSolution Solution { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IVisualLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}