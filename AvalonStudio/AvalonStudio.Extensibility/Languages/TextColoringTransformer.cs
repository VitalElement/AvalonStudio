using Avalonia.Media;
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

        public void Dispose()
        {
            TextTransformations.Clear();
            TextTransformations = null;
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

        public ColorScheme ColorScheme { get; set; }

        protected override void TransformLine(DocumentLine line, ITextRunConstructionContext context)
        {
            var transformsInLine = TextTransformations.FindOverlappingSegments(line);

            foreach (var transform in transformsInLine.OfType<ForegroundTextTransformation>())
            {
                transform.Transform(this, line);
            }

            foreach (var transform in transformsInLine.OfType<OpacityTextTransformation>())
            {
                transform.Transform(this, line);
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

        private TextTransformation GetTextTransformation(object tag, OffsetSyntaxHighlightingData highlight)
        {
            if (highlight is LineColumnSyntaxHighlightingData lineColumnHighlight)
            {
                if (highlight.Type == HighlightType.Unnecessary)
                {
                    return new OpacityTextTransformation(
                        tag,
                        document.GetOffset(lineColumnHighlight.StartLine, lineColumnHighlight.StartColumn),
                        document.GetOffset(lineColumnHighlight.EndLine, lineColumnHighlight.EndColumn),
                        0.5);
                }
                else
                {
                    return new ForegroundTextTransformation(
                        tag,
                        document.GetOffset(lineColumnHighlight.StartLine, lineColumnHighlight.StartColumn),
                        document.GetOffset(lineColumnHighlight.EndLine, lineColumnHighlight.EndColumn),
                        GetBrush(highlight.Type));
                }
            }
            else
            {
                if (highlight.Type == HighlightType.Unnecessary)
                {
                    return new OpacityTextTransformation(
                    tag,
                    highlight.Start,
                    highlight.Start + highlight.Length,
                    0.5);
                }
                else
                {
                    return new ForegroundTextTransformation(
                    tag,
                    highlight.Start,
                    highlight.Start + highlight.Length,
                    GetBrush(highlight.Type));
                }
            }
        }

        public void SetTransformations(object tag, SyntaxHighlightDataList highlightData)
        {
            foreach (var highlight in highlightData)
            {
                if (highlight.Type != HighlightType.None)
                {
                    TextTransformations.Add(GetTextTransformation(tag, highlight));
                }
            }
        }

        public IBrush GetBrush(HighlightType type)
        {
            IBrush result;

            switch (type)
            {
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