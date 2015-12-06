namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Models.LanguageServices;
    using Perspex.Media;

    public class TextColoringTransformer : IDocumentLineTransformer
    {
        public TextColoringTransformer (TextEditor editor)
        {
            this.editor = editor;

            TextTransformations = new TextSegmentCollection<TextTransformation>(editor.TextDocument);            
        }

        private TextEditor editor;

        public void SetTransformations ()
        {
            var transformations = new TextSegmentCollection<TextTransformation>(editor.TextDocument);

            foreach (var transform in editor.SyntaxHighlightingData)
            {
                transformations.Add(new TextTransformation { Foreground = GetBrush(transform.Type), StartOffset = transform.Start, EndOffset = transform.Start + transform.Length });
            }

            TextTransformations = transformations;
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }        

        public Brush GetBrush(HighlightType type)
        {
            Brush result;

            switch(type)
            {
                case HighlightType.Comment:
                    result = editor.CommentBrush;
                    break;

                case HighlightType.Identifier:
                    result= editor.IdentifierBrush;
                    break;

                case HighlightType.Keyword:
                    result = editor.KeywordBrush;
                    break;

                case HighlightType.Literal:
                    result = editor.LiteralBrush;
                    break;

                case HighlightType.Punctuation:
                    result = editor.PunctuationBrush;
                    break;

                case HighlightType.UserType:
                    result = editor.UserTypeBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }

        public void TransformLine(DocumentLine line, FormattedText formattedText)
        {
            var markers = TextTransformations.FindOverlappingSegments(line);

            foreach(var marker in markers)
            {
                var formattedOffset = 0;

                if (marker.StartOffset > line.Offset)
                {
                    formattedOffset = marker.StartOffset - line.Offset;
                }

                formattedText.SetForegroundBrush(marker.Foreground, formattedOffset, marker.EndOffset);
            }
        }
    }
}
