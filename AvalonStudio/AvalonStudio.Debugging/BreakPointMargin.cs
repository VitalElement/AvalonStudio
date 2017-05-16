using Avalonia;
using Avalonia.Input;
using Avalonia.Media;
using AvalonStudio.Platforms;
using Mono.Debugging.Client;
using System;
using System.Linq;

namespace AvalonStudio.TextEditor
{
   /* public class BreakPointMargin : TextViewMargin
    {
        private readonly BreakpointStore _manager;

        private int previewLine;
        private bool previewPointVisible;

        static BreakPointMargin()
        {
            FocusableProperty.OverrideDefaultValue(typeof(BreakPointMargin), true);
        }

        public BreakPointMargin(BreakpointStore manager)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            this._manager = manager;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
        }

        public override void Render(DrawingContext context, TextInfo textInfo)
        {
            Width = textInfo.LineHeight;

            context.FillRectangle(Brush.Parse("#333333"), Bounds);

            if (previewPointVisible)
            {
                context.FillRectangle(Brush.Parse("#E67466"),
                    new Rect((Bounds.Size.Width / 4) - 1,
                        (textInfo.LineHeight * (previewLine - textView.VisualLines.First().DocumentLine.LineNumber)) + (Bounds.Size.Width / 4),
                        Bounds.Size.Width / 1.5, textInfo.LineHeight / 1.5), (float)textInfo.LineHeight);
            }

            foreach (var breakPoint in _manager?.OfType<Breakpoint>().Where(bp => bp.FileName.IsSamePathAs(textView.TextDocument.FileName)))
            {
                context.FillRectangle(Brush.Parse("#FF3737"),
                    new Rect((Bounds.Size.Width / 4) - 1,
                        (textInfo.LineHeight * (breakPoint.Line - textView.VisualLines.First().DocumentLine.LineNumber)) + (Bounds.Size.Width / 4),
                        Bounds.Size.Width / 1.5, textInfo.LineHeight / 1.5), (float)textInfo.LineHeight);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            previewPointVisible = true;

            var offset = textView.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                previewLine = textView.TextDocument.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.
            }

            InvalidateVisual();
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            previewPointVisible = true;

            var offset = textView.GetOffsetFromPoint(e.GetPosition(this));

            if (offset != -1)
            {
                var lineClicked = -1;
                lineClicked = textView.TextDocument.GetLineByOffset(offset).LineNumber; // convert from text line to visual line.

                var currentBreakPoint =
                    _manager.OfType<Breakpoint>().FirstOrDefault(bp => bp.FileName == textView.TextDocument.FileName && bp.Line == lineClicked) as BreakEvent;

                if (currentBreakPoint != null)
                {
                    _manager.Remove(currentBreakPoint);
                }
                else
                {
                    if (!string.IsNullOrEmpty(textView.TextDocument.FileName))
                    {
                        _manager.Add(textView.TextDocument.FileName, lineClicked);
                    }
                }
            }

            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(100, 0);
        }

        protected override void OnPointerLeave(PointerEventArgs e)
        {
            previewPointVisible = false;

            InvalidateVisual();
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            InvalidateVisual();
        }
    }*/
}