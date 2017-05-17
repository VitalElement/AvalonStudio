using Avalonia;
using Avalonia.Controls;
using AvaloniaEdit.Editing;
using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.CodeEditor
{
    public class CodeEditor : AvaloniaEdit.TextEditor
    {
        public CodeEditor()
        {
            var breakpointMargin = new BreakPointMargin(this, IoC.Get<IDebugManager2>().Breakpoints);

            TextArea.LeftMargins.Add(breakpointMargin);

            var lineNumberMargin = new LineNumberMargin(this) { Margin = new Thickness(0, 0, 10, 0) };

            TextArea.LeftMargins.Add(lineNumberMargin);
        }

        public int GetOffsetFromPoint(Point point)
        {
            var position = GetPositionFromPoint(point);

            var offset = position != null ? Document.GetOffset(position.Value.Location) : -1;

            return offset;
        }
    }
}
