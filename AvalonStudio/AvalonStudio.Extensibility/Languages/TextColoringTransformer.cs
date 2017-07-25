using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using AvalonStudio.Extensibility.Editor;
using System;

namespace AvalonStudio.Languages
{
    public class TextColoringTransformer : GenericLineTransformer
    {
        private readonly TextDocument document;

        public TextColoringTransformer(TextDocument document)
        {
            this.document = document;

            TextTransformations = new TextSegmentCollection<TextTransformation>(document);

            CommentBrush = Brush.Parse("#559A3F");
            CallExpressionBrush = Brush.Parse("#DCDCAA");
            OperatorBrush = Brush.Parse("#B4B4B4");
            IdentifierBrush = Brush.Parse("#DCDCDC");
            KeywordBrush = Brush.Parse("#569CD6");
            LiteralBrush = Brush.Parse("#D69D85");
            NumericLiteralBrush = Brush.Parse("#B5CEA8");
            EnumConstantBrush = Brush.Parse("#B5CEA8");
            EnumTypeNameBrush = Brush.Parse("#B5CEA8");
            InterfaceBrush = Brush.Parse("#B5CEA8");

            PunctuationBrush = Brush.Parse("#C8C8C8");
            UserTypeBrush = Brush.Parse("#4EC9B0");
            StructNameBrush = Brush.Parse("#4EC9B0");
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public IBrush OperatorBrush { get; set; }

        public IBrush PunctuationBrush { get; set; }

        public IBrush KeywordBrush { get; set; }

        public IBrush IdentifierBrush { get; set; }

        public IBrush NumericLiteralBrush { get; set; }

        public IBrush LiteralBrush { get; set; }

        public IBrush UserTypeBrush { get; set; }

        public IBrush StructNameBrush { get; set; }

        public IBrush CallExpressionBrush { get; set; }

        public IBrush CommentBrush { get; set; }

        public IBrush EnumConstantBrush { get; set; }

        public IBrush InterfaceBrush { get; set; }

        public IBrush EnumTypeNameBrush { get; set; }

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine)
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                SetTextStyle(line, formattedOffset, transform.Length, transform.Foreground);
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
            });
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

                case HighlightType.Operator:
                    result = OperatorBrush;
                    break;

                case HighlightType.StructName:
                    result = StructNameBrush;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }
    }
}