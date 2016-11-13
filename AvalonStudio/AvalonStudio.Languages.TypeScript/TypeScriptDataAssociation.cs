using Avalonia.Input;
using AvalonStudio.Languages.TypeScript.Rendering;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Rendering;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Languages.TypeScript
{
    internal class TypeScriptDataAssociation
    {
        public TypeScriptDataAssociation(TextDocument textDocument)
        {
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IDocumentLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);
            TextMarkerService = new TextMarkerService(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());
            BackgroundRenderers.Add(TextMarkerService);
        }

        public TextColoringTransformer TextColorizer { get; }
        public TextMarkerService TextMarkerService { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IDocumentLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}