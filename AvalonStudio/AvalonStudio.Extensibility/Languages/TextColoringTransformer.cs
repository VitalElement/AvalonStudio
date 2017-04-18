using Avalonia.Media;
using Avalonia.Threading;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Rendering;
using System;

namespace AvalonStudio.Languages
{
    public class TextColoringTransformer : IDocumentLineTransformer
    {
        private readonly TextDocument document;

        public TextColoringTransformer(TextDocument document)
        {
            this.document = document;

            TextTransformations = new TextSegmentCollection<TextTransformation>(document);

            CommentBrush = Brush.Parse("#559A3F");
            CallExpressionBrush = Brush.Parse("#DCDCAA");
            IdentifierBrush = Brush.Parse("#C8C8C8");
            KeywordBrush = Brush.Parse("#569CD6");
            LiteralBrush = Brush.Parse("#D69D85");
            NumericLiteralBrush = Brush.Parse("#B5CEA8");
            EnumConstantBrush = Brush.Parse("#B5CEA8");
            EnumTypeNameBrush = Brush.Parse("#B5CEA8");
            InterfaceBrush = Brush.Parse("#B5CEA8");

            PunctuationBrush = Brush.Parse("#C8C8C8");
            UserTypeBrush = Brush.Parse("#4EC9B0");
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public IBrush PunctuationBrush { get; set; }

        public IBrush KeywordBrush { get; set; }

        public IBrush IdentifierBrush { get; set; }

        public IBrush NumericLiteralBrush { get; set; }

        public IBrush LiteralBrush { get; set; }

        public IBrush UserTypeBrush { get; set; }

        public IBrush CallExpressionBrush { get; set; }

        public IBrush CommentBrush { get; set; }

        public IBrush EnumConstantBrush { get; set; }

        public IBrush InterfaceBrush { get; set; }

        public IBrush EnumTypeNameBrush { get; set; }

        public event EventHandler<EventArgs> DataChanged;

        public void TransformLine(TextView textView, VisualLine line)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine)
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                line.RenderedText.SetForegroundBrush(transform.Foreground, formattedOffset, transform.Length);
            }
        }

        public void SetTransformations(SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var transformations = new TextSegmentCollection<TextTransformation>(document);

                foreach (var transform in highlightData)
                {
                    if (transform.Type != HighlightType.None)
                    {
                        if (transform is LineColumnSyntaxHighlightingData)
                        {
                            var trans = transform as LineColumnSyntaxHighlightingData;

                            transformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = document.GetOffset(trans.StartLine, trans.StartColumn),
                                EndOffset = document.GetOffset(trans.EndLine, trans.EndColumn)
                            });
                        }
                        else
                        {
                            transformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = transform.Start,
                                EndOffset = transform.Start + transform.Length
                            });
                        }
                    }
                }

                TextTransformations = transformations;

                if (DataChanged != null)
                {
                    DataChanged(this, new EventArgs());
                }
            });
        }

        public void UpdateOffsets(DocumentChangeEventArgs e)
        {
            if (TextTransformations != null)
            {
                TextTransformations.UpdateOffsets(e);
            }
        }

        public IBrush GetBrush(HighlightType type)
        {
            IBrush result;

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

                case HighlightType.NumericLiteral:
                    result = NumericLiteralBrush;
                    break;

                case HighlightType.Punctuation:
                    result = PunctuationBrush;
                    break;

                case HighlightType.InterfaceName:
                    result = InterfaceBrush;
                    break;

                case HighlightType.ClassName:
                    result = UserTypeBrush;
                    break;

                case HighlightType.CallExpression:
                    result = CallExpressionBrush;
                    break;

                case HighlightType.EnumTypeName:
                    result = EnumTypeNameBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }
    }
}