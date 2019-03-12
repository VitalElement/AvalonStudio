using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit.Editing;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.Platforms;
using Mono.Debugging.Client;
using System;
using System.Linq;

namespace AvalonStudio.Controls.Editor
{
    public class BreakPointMargin : AbstractMargin, IDisposable
    {
        private BreakpointStore _manager;
        private CodeEditor _editor;

        private int previewLine;
        private bool previewPointVisible;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin(CodeEditor editor, BreakpointStore manager)
        {
            _manager = manager;
            _editor = editor;
        }

        public override void Render(DrawingContext context)
        {
            if (TextView.VisualLinesValid)
            {
                context.FillRectangle(ColorTheme.CurrentTheme.EditorBackground, Bounds);
                context.DrawLine(new Pen(ColorTheme.CurrentTheme.ControlHigh, 0.5), Bounds.TopRight, Bounds.BottomRight);

                if (TextView.VisualLines.Count > 0)
                {
                    var firstLine = TextView.VisualLines.FirstOrDefault();
                    var height = firstLine.Height;
                    Width = height;
                    var textView = TextView;

                    foreach (var breakPoint in _manager?.OfType<Breakpoint>().Where(bp => bp.FileName.IsSamePathAs(_editor.Editor.SourceFile.FilePath)))
                    {
                        var visualLine = TextView.VisualLines.FirstOrDefault(vl => vl.FirstDocumentLine.LineNumber == breakPoint.Line);

                        if (visualLine != null)
                        {
                            context.FillRectangle(Brush.Parse("#FF3737"),
                            new Rect((Bounds.Size.Width / 4) - 1,
                                 visualLine.GetTextLineVisualYPosition(visualLine.TextLines[0], AvaloniaEdit.Rendering.VisualYPosition.LineTop) + (Bounds.Size.Width / 4) - TextView.VerticalOffset,
                                Bounds.Size.Width / 1.5, height / 1.5), (float)height);
                        }
                    }

                    if (previewPointVisible)
                    {
                        var visualLine = TextView.VisualLines.FirstOrDefault(vl => vl.FirstDocumentLine.LineNumber == previewLine);

                        if (visualLine != null)
                        {
                            context.FillRectangle(Brush.Parse("#E67466"),
                                new Rect((Bounds.Size.Width / 4) - 1,
                                    visualLine.GetTextLineVisualYPosition(visualLine.TextLines[0], AvaloniaEdit.Rendering.VisualYPosition.LineTop) + (Bounds.Size.Width / 4) - TextView.VerticalOffset,
                                    Bounds.Size.Width / 1.5, height / 1.5), (float)height);
                        }
                    }
                }
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            previewPointVisible = true;

            var textView = TextView;

            var offset = _editor.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                previewLine = textView.Document.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.
            }

            InvalidateVisual();
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            previewPointVisible = true;

            var textView = TextView;

            var offset = _editor.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                var lineClicked = -1;
                lineClicked = textView.Document.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.

                var currentBreakPoint =
                    _manager.OfType<Breakpoint>().FirstOrDefault(bp => bp.FileName == _editor.Editor.SourceFile.FilePath && bp.Line == lineClicked) as BreakEvent;

                if (currentBreakPoint != null)
                {
                    _manager.Remove(currentBreakPoint);
                }
                else
                {
                    if (!string.IsNullOrEmpty(_editor.Editor.SourceFile.FilePath))
                    {
                        _manager.Add(_editor.Editor.SourceFile.FilePath, lineClicked);
                    }
                }
            }

            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (TextView != null)
            {
                return new Size(TextView.DefaultLineHeight, 0);
            }

            return new Size(0, 0);
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            previewPointVisible = false;

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            InvalidateVisual();
            e.Handled = true;
        }

        public void Dispose()
        {
            _manager = null;
            _editor = null;
        }
    }
}
