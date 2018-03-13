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

        public TextSegmentCollection<TextTransformation> OpacityTransformations { get; private set; }

        public ColorScheme ColorScheme { get; set; }


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

            if (OpacityTransformations != null)
            {
                transformsInLine = OpacityTransformations.FindOverlappingSegments(line);

                foreach (var transform in transformsInLine)
                {
                    var formattedOffset = 0;

                    if (transform.StartOffset > line.Offset)
                    {
                        formattedOffset = transform.StartOffset - line.Offset;
                    }

                    SetTextOpacity(line, formattedOffset, transform.Length, 0.5);
                }
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

            if(OpacityTransformations != null)
            {
                var toRemove = OpacityTransformations.Where(t => predicate(t)).ToArray();
                
                foreach (var m in toRemove)
                {
                    OpacityTransformations.Remove(m);
                }
            }
        }

        public void AddOpacityTransformations(object tag, SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (OpacityTransformations == null)
                {
                    OpacityTransformations = new TextSegmentCollection<TextTransformation>(document);
                }

                RemoveAll(transform => Equals(transform.Tag, tag));

                foreach (var transform in highlightData)
                {
                    if (transform.Type != HighlightType.None)
                    {
                        if (transform is LineColumnSyntaxHighlightingData)
                        {
                            var trans = transform as LineColumnSyntaxHighlightingData;

                            OpacityTransformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = document.GetOffset(trans.StartLine, trans.StartColumn),
                                EndOffset = document.GetOffset(trans.EndLine, trans.EndColumn),
                                Tag = tag,
                                Opacity = transform.Type == HighlightType.Unnecessary ? 0.5 : 1.0
                            });
                        }
                        else
                        {
                            OpacityTransformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = transform.Start,
                                EndOffset = transform.Start + transform.Length,
                                Tag = tag,
                                Opacity = transform.Type == HighlightType.Unnecessary ? 0.5 : 1.0
                            });
                        }
                    }
                }
            });
        }

        public void SetTransformations(object tag, SyntaxHighlightDataList highlightData)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (TextTransformations == null)
                {
                    TextTransformations = new TextSegmentCollection<TextTransformation>(document);
                }

                RemoveAll(transform => Equals(transform.Tag, tag));

                foreach (var transform in highlightData)
                {
                    if (transform.Type != HighlightType.None)
                    {
                        if (transform is LineColumnSyntaxHighlightingData)
                        {
                            var trans = transform as LineColumnSyntaxHighlightingData;

                            TextTransformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = document.GetOffset(trans.StartLine, trans.StartColumn),
                                EndOffset = document.GetOffset(trans.EndLine, trans.EndColumn),
                                Tag = tag
                            });
                        }
                        else
                        {
                            TextTransformations.Add(new TextTransformation
                            {
                                Foreground = GetBrush(transform.Type),
                                StartOffset = transform.Start,
                                EndOffset = transform.Start + transform.Length,
                                Tag = tag
                            });
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