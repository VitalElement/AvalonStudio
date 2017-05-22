using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit.Editing;
using AvalonStudio.Platforms;
using Mono.Debugging.Client;
using System;
using System.Linq;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class BreakPointMargin : AbstractMargin
    {
        private readonly BreakpointStore _manager;
        private readonly CodeEditor _editor;

        private int previewLine;
        private bool previewPointVisible;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin(CodeEditor editor, BreakpointStore manager)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _editor = editor ?? throw new ArgumentNullException(nameof(editor));
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(Brush.Parse("#333333"), Bounds);

            if (TextView.VisualLines.Count > 0)
            {
                var firstLine = TextView.VisualLines.FirstOrDefault();
                var height = firstLine.Height;
                Width = height;
                var textView = TextView;

                foreach (var breakPoint in _manager?.OfType<Breakpoint>().Where(bp => bp.FileName.IsSamePathAs(textView.Document.FileName)))
                {
                    context.FillRectangle(Brush.Parse("#FF3737"),
                        new Rect((Bounds.Size.Width / 4),
                            (height * (breakPoint.Line - textView.VisualLines.First().FirstDocumentLine.LineNumber)) + (Bounds.Size.Width / 4),
                            Bounds.Size.Width / 1.5, height / 1.5), (float)height);
                }

                if (previewPointVisible)
                {
                    context.FillRectangle(Brush.Parse("#E67466"),
                        new Rect((Bounds.Size.Width / 4),
                            (height * (previewLine - textView.VisualLines.First().FirstDocumentLine.LineNumber)) + (Bounds.Size.Width / 4),
                            Bounds.Size.Width / 1.5, height / 1.5), (float)height);
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
                    _manager.OfType<Breakpoint>().FirstOrDefault(bp => bp.FileName == textView.Document.FileName && bp.Line == lineClicked) as BreakEvent;

                if (currentBreakPoint != null)
                {
                    _manager.Remove(currentBreakPoint);
                }
                else
                {
                    if (!string.IsNullOrEmpty(textView.Document.FileName))
                    {
                        _manager.Add(textView.Document.FileName, lineClicked);
                    }
                }
            }

            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if(TextView != null)
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
    }
}
