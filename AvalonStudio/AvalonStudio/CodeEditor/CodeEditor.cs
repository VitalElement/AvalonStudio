using Avalonia;
using Avalonia.Controls;
using AvaloniaEdit.Editing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.CodeEditor
{
    public class CodeEditor : AvaloniaEdit.TextEditor
    {
        public CodeEditor()
        {
            var lineNumberMargin = new LineNumberMargin(this) { Margin = new Thickness(0, 0, 10, 0) };

            TextArea.LeftMargins.Add(lineNumberMargin);
        }
    }
}
