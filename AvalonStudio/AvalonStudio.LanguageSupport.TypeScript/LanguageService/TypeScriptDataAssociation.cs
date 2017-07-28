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
        public TypeScriptDataAssociation(TextDocument textDocument)
        {
            TextDocument = textDocument;
            BackgroundRenderers = new List<IBackgroundRenderer>();
            DocumentLineTransformers = new List<IVisualLineTransformer>();

            TextColorizer = new TextColoringTransformer(textDocument);

            BackgroundRenderers.Add(new BracketMatchingBackgroundRenderer());

            DocumentLineTransformers.Add(TextColorizer);

            Diagnostics = new Subject<TextSegmentCollection<Diagnostic>>();
        }

        public Subject<TextSegmentCollection<Diagnostic>> Diagnostics { get; set; }
        public TextDocument TextDocument { get; set; }
        public TextColoringTransformer TextColorizer { get; }
        public List<IBackgroundRenderer> BackgroundRenderers { get; }
        public List<IVisualLineTransformer> DocumentLineTransformers { get; }
        public EventHandler<KeyEventArgs> KeyUpHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}