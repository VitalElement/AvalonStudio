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
        private TextDocument _document;

        public TextColoringTransformer(TextDocument document)
        {
            TextTransformations = new TextSegmentCollection<TextTransformation>(document);

            _document = document;
        }

        public void Dispose()
        {
            TextTransformations.Disconnect(_document);
            TextTransformations.Clear();
            TextTransformations = null;
            _document = null;
        }

        public TextSegmentCollection<TextTransformation> TextTransformations { get; private set; }

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
                        _document.GetOffset(lineColumnHighlight.StartLine, lineColumnHighlight.StartColumn),
                        _document.GetOffset(lineColumnHighlight.EndLine, lineColumnHighlight.EndColumn),
                        0.5);
                }
                else
                {
                    return new ForegroundTextTransformation(
                        tag,
                        _document.GetOffset(lineColumnHighlight.StartLine, lineColumnHighlight.StartColumn),
                        _document.GetOffset(lineColumnHighlight.EndLine, lineColumnHighlight.EndColumn),
                        GetBrush(highlight.Type), highlight.Type);
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
                    GetBrush(highlight.Type), highlight.Type);
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

        public void RecalculateBrushes ()
        {
            foreach(var transformation in TextTransformations.OfType<ForegroundTextTransformation>())
            {
                transformation.Foreground = GetBrush(transformation.Type);
            }
        }

        public IBrush GetBrush(HighlightType type)
        {
            IBrush result;

            var colorScheme = ColorScheme.CurrentColorScheme;

            switch (type)
            {
                case HighlightType.DelegateName:
                    result = colorScheme.DelegateName;
                    break;

                case HighlightType.Comment:
                    result = colorScheme.Comment;
                    break;

                case HighlightType.Identifier:
                    result = colorScheme.Identifier;
                    break;

                case HighlightType.Keyword:
                    result = colorScheme.Keyword;
                    break;

                case HighlightType.Literal:
                    result = colorScheme.Literal;
                    break;

                case HighlightType.NumericLiteral:
                    result = colorScheme.NumericLiteral;
                    break;

                case HighlightType.Punctuation:
                    result = colorScheme.Punctuation;
                    break;

                case HighlightType.InterfaceName:
                    result = colorScheme.InterfaceType;
                    break;

                case HighlightType.ClassName:
                    result = colorScheme.Type;
                    break;

                case HighlightType.CallExpression:
                    result = colorScheme.CallExpression;
                    break;

                case HighlightType.EnumTypeName:
                    result = colorScheme.EnumType;
                    break;

                case HighlightType.Operator:
                    result = colorScheme.Operator;
                    break;

                case HighlightType.StructName:
                    result = colorScheme.StructName;
                    break;

                default:
                    result = Brushes.Red;
                    break;
            }

            return result;
        }
    }
}