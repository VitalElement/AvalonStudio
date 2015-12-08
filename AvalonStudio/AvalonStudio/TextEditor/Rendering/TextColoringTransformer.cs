namespace AvalonStudio.TextEditor.Rendering
{
    using AvalonStudio.TextEditor.Document;
    using Models.LanguageServices;
    using Perspex;
    using Perspex.Media;

    public class TextColoringTransformer : IDocumentLineTransformer
    {
        public TextColoringTransformer(TextEditor editor)
        {
            this.editor = editor;

            TextTransformations = new TextSegmentCollection<TextTransformation>(editor.TextDocument);
        }

        private TextEditor editor;

        public void SetTransformations()
        {
            var transformations = new TextSegmentCollection<TextTransformation>(editor.TextDocument);

            foreach (var transform in editor.SyntaxHighlightingData)
            {
                transformations.Add(new TextTransformation { Foreground = GetBrush(transform.Type), StartOffset = transform.Start, EndOffset = transform.Start + transform.Length });
            }

            TextTransformations = transformations;
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public void UpdateOffsets(DocumentChangeEventArgs e)
        {
            if (TextTransformations != null)
            {
                TextTransformations.UpdateOffsets(e);
            }
        }

        public Brush GetBrush(HighlightType type)
        {
            Brush result;

            switch (type)
            {
                case HighlightType.Comment:
                    result = editor.CommentBrush;
                    break;

                case HighlightType.Identifier:
                    result = editor.IdentifierBrush;
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

                case HighlightType.CallExpression:
                    result = editor.CallExpressionBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, DocumentLine line, FormattedText formattedText)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine)
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                formattedText.SetForegroundBrush(transform.Foreground, formattedOffset, transform.EndOffset);
            }
        }
    }
}
