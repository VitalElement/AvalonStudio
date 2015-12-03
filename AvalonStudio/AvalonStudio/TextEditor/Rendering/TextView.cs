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
            documentLineTransformers = new List<IDocumentLineTransformer>();
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

        public int GetOffsetFromPoint(Point point)
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

        private void RenderTextBackground (DrawingContext context, DocumentLine line)
        {

        }

        private void RenderTextDecoration (DrawingContext context, DocumentLine line)
        {

        }

        private void RenderText (DrawingContext context, DocumentLine line)
        {
            var formattedText = new FormattedText(TextDocument.GetText(line.Offset, line.EndOffset - line.Offset), "Consolas", 14, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);            

            foreach(var lineTransformer in DocumentLineTransformers)
            {
                lineTransformer.ColorizeLine(line, formattedText);
            }
            
            context.DrawText(Brushes.WhiteSmoke, VisualLineGeometryBuilder.GetTextPosition(this, line.Offset).TopLeft, formattedText);
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
                    RenderTextBackground(context, line);

                    // Render text layer.
                    RenderText(context, line);

                    // Render text decoration layer.
                    RenderTextDecoration(context, line);
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
        }

        public int GetLine(int caretIndex)
        {
            return TextDocument.GetLineByOffset(caretIndex).LineNumber;
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

        private List<IDocumentLineTransformer> documentLineTransformers;
        public List<IDocumentLineTransformer> DocumentLineTransformers
        {
            get { return documentLineTransformers; }
            set { documentLineTransformers = value; }
        }

    }   
}
