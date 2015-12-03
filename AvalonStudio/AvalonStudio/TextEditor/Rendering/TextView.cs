namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.VisualTree;
    using Rendering;
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

        public static readonly PerspexProperty<Brush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<TextBlock>();


        private readonly DispatcherTimer _caretTimer;

        private bool _caretBlink;

        private IObservable<bool> _canScrollHorizontally;

        public static readonly PerspexProperty<TextDocument> TextDocumentProperty =
            PerspexProperty.Register<TextView, TextDocument>("TextDocument");

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); }
        }

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

            backgroundRenderers = new List<IBackgroundRenderer>();
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
            int result = -1;

            if(TextDocument != null)
            {
                var column = Math.Ceiling(point.X / CharSize.Width);
                var line = Math.Ceiling(point.Y / CharSize.Height);

                result = TextDocument.GetOffset((int)line, (int)column);
            }

            return result;            
        }

        private const string FontFamily = "Consolas";
        private const double FontSize = 14;
        public Size CharSize = new Size();

        private void GenerateTextProperties()
        {
            var formattedText = new FormattedText("x", FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);
            CharSize = formattedText.Measure();
        }

        private void RenderBackground (DrawingContext context)
        {
            foreach(var renderer in BackgroundRenderers)
            {
                renderer.Draw(this, context);
            }
        }

        public override void Render(DrawingContext context)
        {
            GenerateTextProperties();

            if (TextDocument != null)
            {
                // Render background layer.
                RenderBackground(context);

                foreach (var line in TextDocument.Lines)
                {
                    // Render text background layer.

                    // Render text layer.

                    // Render text decoration layer.

                }

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
                    var charPos = VisualLineGeometryBuilder.GetTextPosition(this, CaretIndex);
                    var x = Math.Floor(charPos.X) + 0.5;
                    var y = Math.Floor(charPos.Y) + 0.5;
                    var b = Math.Ceiling(charPos.Bottom) - 0.5;

                    context.DrawLine(
                        new Pen(caretBrush, 1),
                        new Point(x, y),
                        new Point(x, b));
                }
            }



            if (TextDocument != null && TextDocument.LineCount > 0)
            {                
                for(int i = 0; i <TextDocument.LineCount; i++)
                {
                    var line = TextDocument.Lines[i];

                    var formattedText = new FormattedText(TextDocument.GetText(line.Offset, line.EndOffset - line.Offset), "Consolas", 14, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

                    context.DrawText(Brushes.WhiteSmoke, new Point(0, CharSize.Height * i), formattedText);
                }                
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
            GenerateTextProperties();

            if (TextDocument != null)
            {
                //return base.MeasureOverride(availableSize);
                return new Size(availableSize.Width, TextDocument.LineCount * CharSize.Height);
            }
            else
            {
                return base.MeasureOverride(availableSize);
            }            

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

        private List<IBackgroundRenderer> backgroundRenderers;
        public List<IBackgroundRenderer> BackgroundRenderers
        {
            get { return backgroundRenderers; }
            set { backgroundRenderers = value; }
        }

    }   
}
