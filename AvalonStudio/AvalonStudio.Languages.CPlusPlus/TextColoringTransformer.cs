namespace AvalonStudio.Languages.CPlusPlus.Rendering
{
    using System;
    using Languages;
    using Perspex;
    using Perspex.Media;
    using Perspex.Threading;
    using TextEditor.Document;
    using TextEditor.Rendering;

    public class TextColoringTransformer : IDocumentLineTransformer
    {
        public TextColoringTransformer(TextDocument document)
        {
            this.document = document;

            TextTransformations = new TextSegmentCollection<TextTransformation>(document);

            CommentBrush = Brush.Parse("#559A3F");
            CallExpressionBrush = Brush.Parse("Yellow");
            IdentifierBrush = Brush.Parse("#D4D4D4");
            KeywordBrush = Brush.Parse("#569CD6");
            LiteralBrush = Brush.Parse("#D69D85");
            PunctuationBrush = Brush.Parse("#D4D4D4");
            UserTypeBrush = Brush.Parse("#4BB289");
        }

        private TextDocument document;

        public event EventHandler<EventArgs> DataChanged;

        public void SetTransformations(SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var transformations = new TextSegmentCollection<TextTransformation>(document);

                foreach (var transform in highlightData)
                {
                    transformations.Add(new TextTransformation { Foreground = GetBrush(transform.Type), StartOffset = transform.Start, EndOffset = transform.Start + transform.Length });
                }

                TextTransformations = transformations;

                if(this.DataChanged != null)
                {
                    this.DataChanged(this, new EventArgs());
                }
            });
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public void UpdateOffsets(DocumentChangeEventArgs e)
        {
            if (TextTransformations != null)
            {
                TextTransformations.UpdateOffsets(e);
            }
        }

        public Brush PunctuationBrush { get; set; }

        public Brush KeywordBrush { get; set; }

        public Brush IdentifierBrush { get; set; }

        public Brush LiteralBrush { get; set; }

        public Brush UserTypeBrush { get; set; }

        public Brush CallExpressionBrush { get; set; }

        public Brush CommentBrush { get; set; }

        public Brush GetBrush(HighlightType type)
        {
            Brush result;

            switch (type)
            {
                case HighlightType.Comment:
                    result = CommentBrush;
                    break;

                case HighlightType.Identifier:
                    result = IdentifierBrush;
                    break;

                case HighlightType.Keyword:
                    result = KeywordBrush;
                    break;

                case HighlightType.Literal:
                    result = LiteralBrush;
                    break;

                case HighlightType.Punctuation:
                    result = PunctuationBrush;
                    break;

                case HighlightType.UserType:
                    result = UserTypeBrush;
                    break;

                case HighlightType.CallExpression:
                    result = CallExpressionBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }

        public void TransformLine(TextView textView, DrawingContext context, Rect lineBounds, VisualLine line)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine)
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                line.RenderedText.SetForegroundBrush(transform.Foreground, formattedOffset, transform.EndOffset);
            }
        }
    }
}
