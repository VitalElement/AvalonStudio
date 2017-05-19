using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public static class EditorExtensions
    {
        public static string GetPreviousWordAtIndex(this AvaloniaEdit.TextEditor editor, int index)
        {
            var lastWordIndex = TextUtilities.GetNextCaretPosition(editor.Document, index, LogicalDirection.Backward, CaretPositioningMode.WordBorder);

            if (lastWordIndex >= 0 && editor.Document.GetLocation(lastWordIndex).Line == editor.Document.GetLocation(index).Line)
            {
                return editor.GetWordAtIndex(lastWordIndex);
            }
            else
            {
                return editor.GetWordAtIndex(index);
            }
        }

        public static bool IsSymbol(this string text)
        {
            var result = false;

            if (!string.IsNullOrEmpty(text) && (char.IsLetter(text[0]) || text[0] == '_'))
            {
                result = true;
            }

            return result;
        }

        public static string GetWordAtIndex(this AvaloniaEdit.TextEditor editor, int index)
        {
            var result = string.Empty;

            if (index >= 0 && editor.Document.TextLength > index)
            {
                var start = index;

                var currentChar = editor.Document.GetCharAt(index);
                var prevChar = '\0';

                if (index > 0)
                {
                    prevChar = editor.Document.GetCharAt(index - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != CharacterClass.LineTerminator && prevChar != ' ' && TextUtilities.GetCharacterClass(prevChar) != CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(editor.Document, index, LogicalDirection.Backward, CaretPositioningMode.WordStart);
                }

                var end = TextUtilities.GetNextCaretPosition(editor.Document, start, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    var word = editor.Document.GetText(start, end - start).Trim();

                    if (word.IsSymbol())
                    {
                        result = word;
                    }
                }
            }

            return result;
        }

        private static IEnumerable<Tuple<int, int>> GetOffsetForLinesInSegmentOnScreen(TextView textView, ISegment segment,
            bool extendToFullWidthAtLineEnd = false)
        {
            var segmentStart = segment.Offset;
            var segmentEnd = segment.Offset + segment.Length;

            if (segmentStart > textView.Document.TextLength)
            {
                segmentStart = textView.Document.TextLength;
            }

            if (segmentEnd > textView.Document.TextLength)
            {
                segmentEnd = textView.Document.TextLength;
            }

            var start = new TextViewPosition(textView.Document.GetLocation(segmentStart));
            var end = new TextViewPosition(textView.Document.GetLocation(segmentEnd));

            foreach (var line in textView.VisualLines)
            {
                if (!line.IsDisposed)
                {
                    if (line.FirstDocumentLine.Offset > segmentEnd)
                    {
                        break;
                    }

                    if (line.LastDocumentLine.EndOffset < segmentStart)
                    {
                        continue;
                    }

                    // find start and begining in current line.
                    var lineStartOffset = line.FirstDocumentLine.Offset;

                    if (segment.Offset > line.FirstDocumentLine.Offset)
                    {
                        lineStartOffset = line.FirstDocumentLine.Offset + (segment.Offset - line.FirstDocumentLine.Offset);
                    }

                    var lineEndOffset = line.LastDocumentLine.EndOffset;

                    if (segment.EndOffset < line.LastDocumentLine.EndOffset)
                    {
                        lineEndOffset = line.LastDocumentLine.EndOffset - (line.LastDocumentLine.EndOffset - segment.EndOffset);
                    }

                    if (!extendToFullWidthAtLineEnd)
                    {
                        var text = textView.Document.GetText(lineStartOffset, lineEndOffset - lineStartOffset);

                        int offset = text.TakeWhile(c => char.IsWhiteSpace(c)).Count();

                        lineStartOffset += offset;

                        offset = text.Reverse().TakeWhile(c => char.IsWhiteSpace(c)).Count();

                        lineEndOffset -= offset;
                    }

                    // generate rect for section in this line.
                    yield return new Tuple<int, int>(lineStartOffset, lineEndOffset);
                }
            }
        }

        private static IEnumerable<Tuple<int, int>> GetOffsetForLinesInSegmentInDocument(TextDocument textDocument,
            ISegment segment, bool extendToFullWidthAtLineEnd = false)
        {
            var segmentStart = segment.Offset;
            var segmentEnd = segment.Offset + segment.Length;

            if (segmentStart > textDocument.TextLength)
            {
                segmentStart = textDocument.TextLength;
            }

            if (segmentEnd > textDocument.TextLength)
            {
                segmentEnd = textDocument.TextLength;
            }

            var start = new TextViewPosition(textDocument.GetLocation(segmentStart));
            var end = new TextViewPosition(textDocument.GetLocation(segmentEnd));

            foreach (var line in textDocument.Lines)
            {
                if (!line.IsDeleted)
                {
                    if (line.Offset > segmentEnd)
                    {
                        break;
                    }

                    if (line.EndOffset < segmentStart)
                    {
                        continue;
                    }

                    // find start and begining in current line.
                    var lineStartOffset = line.Offset;

                    if (segment.Offset > line.Offset)
                    {
                        lineStartOffset = line.Offset + (segment.Offset - line.Offset);
                    }

                    var lineEndOffset = line.EndOffset;

                    if (segment.EndOffset < line.EndOffset)
                    {
                        lineEndOffset = line.EndOffset - (line.EndOffset - segment.EndOffset);
                    }

                    if (!extendToFullWidthAtLineEnd)
                    {
                        var text = textDocument.GetText(lineStartOffset, lineEndOffset - lineStartOffset);

                        int offset = text.TakeWhile(c => char.IsWhiteSpace(c)).Count();

                        lineStartOffset += offset;

                        offset = text.Reverse().TakeWhile(c => char.IsWhiteSpace(c)).Count();

                        lineEndOffset -= offset;
                    }

                    // generate rect for section in this line.
                    yield return new Tuple<int, int>(lineStartOffset, lineEndOffset);
                }
            }
        }

        public static IEnumerable<IDocumentLine> GetLinesForSegmentInDocument(this TextDocument textDocument, ISegment segment,
            bool extendToFullWidthAtLineEnd = false)
        {
            foreach (var tuple in GetOffsetForLinesInSegmentInDocument(textDocument, segment, extendToFullWidthAtLineEnd))
            {
                yield return textDocument.GetLineByOffset(tuple.Item1);
            }
        }

        public static IEnumerable<IDocumentLine> GetLinesForSegmentOnScreen(this TextView textView, ISegment segment,
            bool extendToFullWidthAtLineEnd = false)
        {
            foreach (var tuple in GetOffsetForLinesInSegmentOnScreen(textView, segment, extendToFullWidthAtLineEnd))
            {
                yield return textView.Document.GetLineByOffset(tuple.Item1);
            }
        }
    }
}
