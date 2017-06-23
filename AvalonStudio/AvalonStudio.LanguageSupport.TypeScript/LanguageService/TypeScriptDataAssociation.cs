using Avalonia.Input;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Languages;
using System;
using System.Collections.Generic;

namespace AvalonStudio.LanguageSupport.TypeScript.LanguageService
{
    internal class TypeScriptDataAssociation
    {
        public TypeScriptDataAssociation(TextDocument textDocument)
        {
            TextDocument = textDocument;
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IVisualLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);

            DocumentLineTransformers.Add(TextColorizer);
        }

        public TextDocument TextDocument { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IVisualLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}