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

    public class TextView : Control
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

            //_canScrollHorizontally = GetObservable(TextWrappingProperty)
            //    .Select(x => x == TextWrapping.NoWrap);

            //Observable.Merge(
            //    GetObservable(SelectionStartProperty),
            //    GetObservable(SelectionEndProperty))
            //    .Subscribe(_ => InvalidateFormattedText());

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
            return 1;
            //var hit = FormattedText.HitTestPoint(point);
            //return hit.TextPosition + (hit.IsTrailing ? 1 : 0);
        }

        public override void Render(DrawingContext context)
        {
            if (editor.TextDocument != null && editor.TextDocument.LineCount > 0)
            {                
                var formattedText = new FormattedText(editor.TextDocument.GetText(editor.TextDocument.Lines[0].Offset, editor.TextDocument.Lines[0].EndOffset), "Consolas", 14, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

                context.DrawText(Brushes.WhiteSmoke, new Point(0, 0), formattedText);
            }
        }

        public int GetLine(int caretIndex)
        {
            //var lines = FormattedText.GetLines().ToList();

            //int pos = 0;
            //int i;

            //for (i = 0; i < lines.Count; ++i)
            //{
            //    var line = lines[i];
            //    pos += line.Length;

            //    if (pos > caretIndex)
            //    {
            //        break;
            //    }
            //}

            return 1;
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

               // var rect = FormattedText.HitTestTextPosition(caretIndex);
               // this.BringIntoView(rect);
            }
        }

        //protected override FormattedText CreateFormattedText(Size constraint)
        //{
        //    var result = base.CreateFormattedText(constraint);
        //    var selectionStart = SelectionStart;
        //    var selectionEnd = SelectionEnd;
        //    var start = Math.Min(selectionStart, selectionEnd);
        //    var length = Math.Max(selectionStart, selectionEnd) - start;

        //    if (length > 0)
        //    {
        //        result.SetForegroundBrush(Brushes.White, start, length);
        //    }

        //    return result;
        //}

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
            //var text = Text;

            //if (!string.IsNullOrWhiteSpace(text))
            //{
            //    return base.MeasureOverride(availableSize);
            //}
            //else
            //{
            //    //// TODO: Pretty sure that measuring "X" isn't the right way to do this...
            //    //using (var formattedText = new FormattedText(
            //    //    "X",
            //    //    FontFamily,
            //    //    FontSize,
            //    //    FontStyle,
            //    //    TextAlignment.Left,
            //    //    FontWeight))
            //    //{
            //    //    return formattedText.Measure();
            //    //}
            //}
        }

        private void CaretTimerTick(object sender, EventArgs e)
        {
            _caretBlink = !_caretBlink;
            InvalidateVisual();
        }
    }   
}
