using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.CodeEditor;
using AvalonStudio.Extensibility.Editor;
using System;
using System.Linq;

namespace AvalonStudio.Languages
{
    public class TextColoringTransformer : GenericLineTransformer
    {
        private readonly TextDocument document;

        public TextColoringTransformer(TextDocument document)
        {
            this.document = document;

            TextTransformations = new TextSegmentCollection<TextTransformation>(document);

            ColorScheme = ColorScheme.Default;
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public ColorScheme ColorScheme { get; set; }


        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine.OfType<ForegroundTextTransformation>())
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                SetTextStyle(line, formattedOffset, transform.Length, transform.Foreground);
            }

            foreach (var transform in transformsInLine.OfType<OpacityTextTransformation>())
            {
                var formattedOffset = 0;

                if (transform.StartOffset > line.Offset)
                {
                    formattedOffset = transform.StartOffset - line.Offset;
                }

                SetTextOpacity(line, formattedOffset, transform.Length, transform.Opacity);
            }
        }

        public void RemoveAll(Predicate<TextTransformation> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            if (TextTransformations != null)
            {
                var toRemove = TextTransformations.Where(t => predicate(t)).ToArray();

                foreach (var m in toRemove)
                {
                    TextTransformations.Remove(m);
                }
            }
        }

        public void AddOpacityTransformations(object tag, SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                RemoveAll(transform => Equals(transform.Tag, tag));

                foreach (var transform in highlightData)
                {
                    if (transform.Type != HighlightType.None)
                    {
                        if (transform is LineColumnSyntaxHighlightingData)
                        {
                            var trans = transform as LineColumnSyntaxHighlightingData;

                            TextTransformations.Add(new OpacityTextTransformation(
                                tag,
                                document.GetOffset(trans.StartLine, trans.StartColumn),
                                document.GetOffset(trans.EndLine, trans.EndColumn),
                                0.5));
                        }
                        else
                        {
                            TextTransformations.Add(new OpacityTextTransformation(
                                tag,
                                transform.Start,
                                transform.Start + transform.Length,
                                0.5));
                        }
                    }
                }
            });
        }

        public void SetTransformations(object tag, SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                RemoveAll(transform => Equals(transform.Tag, tag));

                foreach (var transform in highlightData)
                {
                    if (transform.Type != HighlightType.None)
                    {
                        if (transform is LineColumnSyntaxHighlightingData)
                        {
                            var trans = transform as LineColumnSyntaxHighlightingData;

                            TextTransformations.Add(new ForegroundTextTransformation(
                                tag,
                                document.GetOffset(trans.StartLine, trans.StartColumn),
                                document.GetOffset(trans.EndLine, trans.EndColumn),
                                GetBrush(transform.Type)));
                        }
                        else
                        {
                            TextTransformations.Add(new ForegroundTextTransformation(
                                tag,
                                transform.Start,
                                transform.Start + transform.Length,
                                GetBrush(transform.Type)));
                        }
                    }
                }
            });
        }

        public IBrush GetBrush(HighlightType type)
        {
            IBrush result;

            switch (type)
            {
                case HighlightType.Unnecessary:
                    result = null;
                    break;

                case HighlightType.DelegateName:
                    result = ColorScheme.DelegateName;
                    break;

                case HighlightType.Comment:
                    result = ColorScheme.Comment;
                    break;

                case HighlightType.Identifier:
                    result = ColorScheme.Identifier;
                    break;

                case HighlightType.Keyword:
                    result = ColorScheme.Keyword;
                    break;

                case HighlightType.Literal:
                    result = ColorScheme.Literal;
                    break;

                case HighlightType.NumericLiteral:
                    result = ColorScheme.NumericLiteral;
                    break;

                case HighlightType.Punctuation:
                    result = ColorScheme.Punctuation;
                    break;

                case HighlightType.InterfaceName:
                    result = ColorScheme.InterfaceType;
                    break;

                case HighlightType.ClassName:
                    result = ColorScheme.Type;
                    break;

                case HighlightType.CallExpression:
                    result = ColorScheme.CallExpression;
                    break;

                case HighlightType.EnumTypeName:
                    result = ColorScheme.EnumType;
                    break;

                case HighlightType.Operator:
                    result = ColorScheme.Operator;
                    break;

                case HighlightType.StructName:
                    result = ColorScheme.StructName;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }
    }
}