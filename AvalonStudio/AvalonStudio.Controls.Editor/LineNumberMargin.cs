using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;
using System;
using System.Globalization;

namespace AvalonStudio.Controls.Editor
{
    /// <summary>
    /// Margin showing line numbers.
    /// </summary>
    public class LineNumberMargin : AbstractMargin, IDisposable
    {
        private TextArea _textArea;
        private CodeEditor _editor;
        private const int MinLineNumberWidth = 3;
        private const int RightMarginChars = 0;
        private double RightMarginSize = 0;

        public LineNumberMargin(CodeEditor editor)
        {
            _editor = editor;
            _textArea = _editor.TextArea;
            Foreground = Brush.Parse("#2691AF");
            SelectedLineForeground = Brush.Parse("#2691AF");
        }

        /// <summary>
        /// The typeface used for rendering the line number margin.
        /// This field is calculated in MeasureOverride() based on the FontFamily etc. properties.
        /// </summary>
        protected FontFamily Typeface { get; set; }

        /// <summary>
        /// The font size used for rendering the line number margin.
        /// This field is calculated in MeasureOverride() based on the FontFamily etc. properties.
        /// </summary>
        protected double EmSize;

        public IBrush Background { get; set; }
        public IBrush Foreground { get; set; }
        public IBrush SelectedLineForeground { get; set; }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            Typeface = GetValue(TextBlock.FontFamilyProperty);
            EmSize = GetValue(TextBlock.FontSizeProperty);

            var text = TextFormatterFactory.CreateFormattedText(
                this,
                new string('9', MaxLineNumberLength),
                Typeface,
                EmSize,
                GetValue(TemplatedControl.ForegroundProperty)
            );

            var textRight = TextFormatterFactory.CreateFormattedText(
                this,
                new string('9', RightMarginChars),
                Typeface,
                EmSize,
                GetValue(TemplatedControl.ForegroundProperty)
            );

            RightMarginSize = textRight.Bounds.Width;

            return new Size(text.Bounds.Width + RightMarginSize, 0);
        }

        /// <inheritdoc/>
        public override void Render(DrawingContext drawingContext)
        {
            drawingContext.FillRectangle(Background, new Rect(Bounds.Size));

            if (_editor.Document != null)
            {
                var textView = TextView;
                var renderSize = Bounds.Size;
                if (textView != null && textView.VisualLinesValid)
                {
                    int currentLine = -1;

                    if (_editor.SelectionLength == 0 && _editor.CaretOffset >= 0 && _editor.CaretOffset <= _editor.Document.TextLength)
                    {
                        currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset).LineNumber;
                    }

                    foreach (var line in textView.VisualLines)
                    {
                        var lineNumber = line.FirstDocumentLine.LineNumber;

                        var foreground = lineNumber != currentLine ? Foreground : SelectedLineForeground;

                        var text = TextFormatterFactory.CreateFormattedText(
                            this,
                            lineNumber.ToString(CultureInfo.CurrentCulture),
                            Typeface, EmSize, foreground
                        );

                        var y = line.GetTextLineVisualYPosition(line.TextLines[0], VisualYPosition.TextTop);
                        drawingContext.DrawText(foreground, new Point((renderSize.Width - RightMarginSize) - text.Bounds.Width, y - textView.VerticalOffset),
                            text);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDocumentChanged(TextDocument oldDocument, TextDocument newDocument)
        {
            if (oldDocument != null)
            {
                TextDocumentWeakEventManager.LineCountChanged.RemoveHandler(oldDocument, OnDocumentLineCountChanged);
            }
            base.OnDocumentChanged(oldDocument, newDocument);
            if (newDocument != null)
            {
                TextDocumentWeakEventManager.LineCountChanged.AddHandler(newDocument, OnDocumentLineCountChanged);
            }
            OnDocumentLineCountChanged();
        }

        private void OnDocumentLineCountChanged(object sender, EventArgs e)
        {
            OnDocumentLineCountChanged();
        }

        /// <summary>
        /// Maximum length of a line number, in characters
        /// </summary>
        protected int MaxLineNumberLength = MinLineNumberWidth;

        private void OnDocumentLineCountChanged()
        {
            var documentLineCount = Document?.LineCount ?? 1;
            var newLength = documentLineCount.ToString(CultureInfo.CurrentCulture).Length;

            if (newLength < MinLineNumberWidth)
            {
                newLength = MinLineNumberWidth;
            }

            if (newLength != MaxLineNumberLength)
            {
                MaxLineNumberLength = newLength;
                InvalidateMeasure();
            }
        }

        private AnchorSegment _selectionStart;
        private bool _selecting;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

            if (!e.Handled && e.MouseButton == MouseButton.Left && TextView != null && _textArea != null)
            {
                e.Handled = true;
                _textArea.Focus();

                var currentSeg = GetTextLineSegment(e);
                if (currentSeg == SimpleSegment.Invalid)
                    return;
                _textArea.Caret.Offset = currentSeg.Offset + currentSeg.Length;
                e.Device.Capture(this);
                if (e.Device.Captured == this)
                {
                    _selecting = true;
                    _selectionStart = new AnchorSegment(Document, currentSeg.Offset, currentSeg.Length);
                    if (e.InputModifiers.HasFlag(InputModifiers.Shift))
                    {
                        if (_textArea.Selection is SimpleSelection simpleSelection)
                            _selectionStart = new AnchorSegment(Document, simpleSelection.SurroundingSegment);
                    }
                    _textArea.Selection = Selection.Create(_textArea, _selectionStart);
                    if (e.InputModifiers.HasFlag(InputModifiers.Shift))
                    {
                        ExtendSelection(currentSeg);
                    }
                    _textArea.Caret.BringCaretToView(5.0);
                }
            }
        }

        private SimpleSegment GetTextLineSegment(PointerEventArgs e)
        {
            var pos = e.GetPosition(TextView);
            pos = new Point(0, pos.Y.CoerceValue(0, TextView.Bounds.Height) + TextView.VerticalOffset);
            var vl = TextView.GetVisualLineFromVisualTop(pos.Y);
            if (vl == null)
                return SimpleSegment.Invalid;
            var tl = vl.GetTextLineByVisualYPosition(pos.Y);
            var visualStartColumn = vl.GetTextLineVisualStartColumn(tl);
            var visualEndColumn = visualStartColumn + tl.Length;
            var relStart = vl.FirstDocumentLine.Offset;
            var startOffset = vl.GetRelativeOffset(visualStartColumn) + relStart;
            var endOffset = vl.GetRelativeOffset(visualEndColumn) + relStart;
            if (endOffset == vl.LastDocumentLine.Offset + vl.LastDocumentLine.Length)
                endOffset += vl.LastDocumentLine.DelimiterLength;
            return new SimpleSegment(startOffset, endOffset - startOffset);
        }

        private void ExtendSelection(SimpleSegment currentSeg)
        {
            if (currentSeg.Offset < _selectionStart.Offset)
            {
                _textArea.Caret.Offset = currentSeg.Offset;
                _textArea.Selection = Selection.Create(_textArea, currentSeg.Offset, _selectionStart.Offset + _selectionStart.Length);
            }
            else
            {
                _textArea.Caret.Offset = currentSeg.Offset + currentSeg.Length;
                _textArea.Selection = Selection.Create(_textArea, _selectionStart.Offset, currentSeg.Offset + currentSeg.Length);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (_selecting && _textArea != null && TextView != null)
            {
                e.Handled = true;
                var currentSeg = GetTextLineSegment(e);
                if (currentSeg == SimpleSegment.Invalid)
                    return;
                ExtendSelection(currentSeg);
                _textArea.Caret.BringCaretToView(5.0);
            }
            base.OnPointerMoved(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (_selecting)
            {
                _selecting = false;
                _selectionStart = null;
                e.Device.Capture(null);
                e.Handled = true;
            }
            base.OnPointerReleased(e);
        }

        public void Dispose()
        {
            _editor = null;
            _textArea = null;
        }
    }
}
