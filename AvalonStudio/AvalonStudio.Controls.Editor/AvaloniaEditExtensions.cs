using Avalonia;
using AvaloniaEdit;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;

namespace AvalonStudio.Controls.Editor
{
    internal static class AvaloniaEditExtensions
    {
        public static Point GetPosition(this TextView textView, int line, int column)
        {
            var visualPosition = textView.GetVisualPosition(
                new TextViewPosition(line, column), VisualYPosition.LineBottom) - textView.ScrollOffset;
            return visualPosition;
        }

        public static int FindPreviousWordStart(this ITextSource textSource, int offset)
        {
            return TextUtilities.GetNextCaretPosition(textSource, offset, LogicalDirection.Backward, CaretPositioningMode.WordStart);
        }        
    }
}
