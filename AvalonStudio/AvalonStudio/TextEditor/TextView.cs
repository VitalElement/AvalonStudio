namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.VisualTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    public class TextView : TextBlock
    {
        public static readonly PerspexProperty<int> CaretIndexProperty =
            TextBox.CaretIndexProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionStartProperty =
            TextBox.SelectionStartProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionEndProperty =
            TextBox.SelectionEndProperty.AddOwner<TextView>();       


        private readonly DispatcherTimer _caretTimer;

        private bool _caretBlink;

        private IObservable<bool> _canScrollHorizontally;

        private TextEditor editor;

        public TextView(TextEditor editor)
        {
            this.editor = editor;

            _caretTimer = new DispatcherTimer();
            _caretTimer.Interval = TimeSpan.FromMilliseconds(500);
            _caretTimer.Tick += CaretTimerTick;

            _canScrollHorizontally = GetObservable(TextWrappingProperty)
                .Select(x => x == TextWrapping.NoWrap);

            Observable.Merge(
                GetObservable(SelectionStartProperty),
                GetObservable(SelectionEndProperty))
                .Subscribe(_ => InvalidateFormattedText());

            GetObservable(CaretIndexProperty)
                .Subscribe(CaretIndexChanged);
        }

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public int SelectionStart
        {
            get { return GetValue(SelectionStartProperty); }
            set { SetValue(SelectionStartProperty, value); }
        }

        public int SelectionEnd
        {
            get { return GetValue(SelectionEndProperty); }
            set { SetValue(SelectionEndProperty, value); }
        }

        public int GetCaretIndex(Point point)
        {
            var hit = FormattedText.HitTestPoint(point);
            return hit.TextPosition + (hit.IsTrailing ? 1 : 0);
        }

        public override void Render(DrawingContext context)
        {
            if (editor.SyntaxHighlightingData != null)
            {
                foreach (var highlightData in editor.SyntaxHighlightingData)
                {
                    Brush brush;

                    switch(highlightData.Type)
                    {
                        case HighlightType.Comment:
                            brush = editor.CommentBrush;
                            break;

                        case HighlightType.Identifier:
                            brush = editor.IdentifierBrush;
                            break;

                        case HighlightType.Keyword:
                            brush = editor.KeywordBrush;
                            break;

                        case HighlightType.Literal:
                            brush = editor.LiteralBrush;
                            break;

                        case HighlightType.Punctuation:
                            brush = editor.PunctuationBrush;
                            break;

                        case HighlightType.UserType:
                            brush = editor.UserTypeBrush;
                            break;

                        default:
                            brush = Foreground;
                            break;
                    }
                    
                    FormattedText.SetForegroundBrush(brush, highlightData.Start, highlightData.Length);
                }
            }

            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;

            if (selectionStart != selectionEnd)
            {
                var start = Math.Min(selectionStart, selectionEnd);
                var length = Math.Max(selectionStart, selectionEnd) - start;
                var rects = FormattedText.HitTestTextRange(start, length);                

                var brush = new SolidColorBrush(0xff086f9e);

                foreach (var rect in rects)
                {
                    context.FillRectangle(brush, rect);
                }
            }

            // Calculate caret position
            var tlCharPos = FormattedText.HitTestPoint(new Point(0, 0));            
            var charPos = FormattedText.HitTestTextPosition(CaretIndex);
            var x = Math.Floor(charPos.X) + 0.5;
            var y = Math.Floor(charPos.Y) + 0.5;
            var b = Math.Ceiling(charPos.Bottom) - 0.5;
            var lineHeight = b - y;

            // Render Current Line Highlighting.
            if (selectionStart == selectionEnd)
            {
                context.FillRectangle(Brush.Parse("#0e0e0e"), new Rect(0, y, Bounds.Width, lineHeight));
            }
            
            // Render Text
            base.Render(context);

            if (selectionStart == selectionEnd)
            {
                // Render caret
                var backgroundColor = (((Control)TemplatedParent).GetValue(BackgroundProperty) as SolidColorBrush)?.Color;
                var caretBrush = Brushes.Black;

                if (backgroundColor.HasValue)
                {
                    byte red = (byte)~(backgroundColor.Value.R);
                    byte green = (byte)~(backgroundColor.Value.G);
                    byte blue = (byte)~(backgroundColor.Value.B);

                    caretBrush = new SolidColorBrush(Color.FromRgb(red, green, blue));
                }

                if (_caretBlink)
                {
                    context.DrawLine(
                        new Pen(caretBrush, 1),
                        new Point(x, y),
                        new Point(x, b));
                }
            }
        }

        public int GetLine(int caretIndex)
        {
            var lines = FormattedText.GetLines().ToList();

            int pos = 0;
            int i;

            for (i = 0; i < lines.Count; ++i)
            {
                var line = lines[i];
                pos += line.Length;

                if (pos > caretIndex)
                {
                    break;
                }
            }

            return i;
        }

        public void ShowCaret()
        {
            _caretBlink = true;
            _caretTimer.Start();
            InvalidateVisual();
        }

        public void HideCaret()
        {
            _caretBlink = false;
            _caretTimer.Stop();
            InvalidateVisual();
        }

        internal void CaretIndexChanged(int caretIndex)
        {
            if (this.GetVisualParent() != null)
            {
                _caretBlink = true;
                _caretTimer.Stop();
                _caretTimer.Start();
                InvalidateVisual();

                var rect = FormattedText.HitTestTextPosition(caretIndex);
                this.BringIntoView(rect);
            }
        }

        protected override FormattedText CreateFormattedText(Size constraint)
        {
            var result = base.CreateFormattedText(constraint);
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;
            var start = Math.Min(selectionStart, selectionEnd);
            var length = Math.Max(selectionStart, selectionEnd) - start;

            if (length > 0)
            {
                result.SetForegroundBrush(Brushes.White, start, length);
            }

            return result;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var text = Text;

            if (!string.IsNullOrWhiteSpace(text))
            {
                return base.MeasureOverride(availableSize);
            }
            else
            {
                // TODO: Pretty sure that measuring "X" isn't the right way to do this...
                using (var formattedText = new FormattedText(
                    "X",
                    FontFamily,
                    FontSize,
                    FontStyle,
                    TextAlignment.Left,
                    FontWeight))
                {
                    return formattedText.Measure();
                }
            }
        }

        private void CaretTimerTick(object sender, EventArgs e)
        {
            _caretBlink = !_caretBlink;
            InvalidateVisual();
        }
    }   
}
