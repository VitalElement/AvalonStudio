namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.VisualTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    public class TextView : Panel, IScrollable
    {
        private Grid mainGrid;
        private Grid textViewGrid;
        private StackPanel margins;

        #region Constructors
        static TextView()
        {
            AffectsMeasure(TextDocumentProperty);
        }

        public TextView()
        {
            _caretTimer = new DispatcherTimer();
            _caretTimer.Interval = TimeSpan.FromMilliseconds(500);
            _caretTimer.Tick += CaretTimerTick;

            GetObservable(CaretIndexProperty)
                .Subscribe(CaretIndexChanged);

            backgroundRenderers = new List<IBackgroundRenderer>();
            documentLineTransformers = new List<IDocumentLineTransformer>();

            mainGrid = new Grid();
            mainGrid.ColumnDefinitions = new ColumnDefinitions("Auto, *");

            margins = new StackPanel();
            margins.Orientation = Orientation.Horizontal;

            textViewGrid = new Grid();

            mainGrid.Children.Add(margins);

            this.Children.Add(mainGrid);

            VisualLines = new List<VisualLine>();
        }
        #endregion

        public TextLocation GetLocation(int offset)
        {
            var documentLocation = TextDocument.GetLocation(offset);

            var result = new TextLocation(documentLocation.Line - firstVisualLine, documentLocation.Column);

            return result;
        }

        #region Perspex Properties
        public static readonly PerspexProperty<TextWrapping> TextWrappingProperty =
           TextBlock.TextWrappingProperty.AddOwner<TextView>();

        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly PerspexProperty<string> FontFamilyProperty = TextEditor.FontFamilyProperty.AddOwner<TextView>();

        public string FontFamily
        {
            get { return GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly PerspexProperty<double> FontSizeProperty = TextEditor.FontSizeProperty.AddOwner<TextView>();

        public double FontSize
        {
            get { return GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsReturnProperty =
            PerspexProperty.Register<TextView, bool>(nameof(AcceptsReturn), defaultValue: true);

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsTabProperty =
            PerspexProperty.Register<TextView, bool>(nameof(AcceptsTab));

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly PerspexProperty<TextDocument> TextDocumentProperty =
            PerspexProperty.Register<TextView, TextDocument>(nameof(TextDocument));

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); InvalidateMeasure(); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>(nameof(BeforeTextChangedCommand));

        public System.Windows.Input.ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> TextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>(nameof(TextChangedCommand));

        public System.Windows.Input.ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<int> CaretIndexProperty =
            PerspexProperty.Register<TextView, int>(nameof(CaretIndex), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay);

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionStartProperty =
            TextBox.SelectionStartProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionEndProperty =
            TextBox.SelectionEndProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<Brush> ForegoundProperty =
            TextBlock.ForegroundProperty.AddOwner<TextView>();

        public Brush Foreground
        {
            get { return GetValue(ForegoundProperty); }
            set { SetValue(ForegoundProperty, value); }
        }

        public static readonly PerspexProperty<Brush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<TextView>();

        public Brush Background
        {
            get { return GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
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
        #endregion

        #region Properties
        public Size CharSize { get; set; }

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

        public Action InvalidateScroll
        {
            get;
            set;
        }

        private Size extent;
        public Size Extent { get { return extent; } }

        int firstVisualLine = 0;

        private Vector offset;
        public Vector Offset
        {
            get { return offset; }
            set
            {
                offset = value;

                firstVisualLine = (int)(offset.Y);

                InvalidateVisual();
            }
        }

        private Size viewport;
        public Size Viewport
        {
            get { return viewport; }
        }
        #endregion

        #region Control Overrides

        public override void Render(DrawingContext context)
        {
            if (TextDocument != null)
            {
                GenerateTextProperties();
                GenerateVisualLines();

                // Render background layer.
                RenderBackground(context);
               
                foreach (var line in VisualLines)
                {
                    // Render text background layer.
                    RenderTextBackground(context, line);
                   
                    // Render text layer.
                    RenderText(context, line);
                    
                    // Render text decoration layer.
                    RenderTextDecoration(context, line);
                }

                RenderCaret(context);              
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var result = finalSize;

            if (TextDocument != null)
            {
                GenerateTextProperties();

                viewport = new Size(finalSize.Width, finalSize.Height / CharSize.Height);
                extent = new Size(finalSize.Width, TextDocument.LineCount + 20);


                InvalidateScroll.Invoke();
                // scan visual lines, find largest for width....
                //return base.MeasureOverride(availableSize);
                //return new Size(1000, (TextDocument.LineCount) * CharSize.Height);                
            }

            return result;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size result = availableSize;

            if (TextDocument != null)
            {
                GenerateTextProperties();

                result = new Size(availableSize.Width, TextDocument.LineCount * CharSize.Height);
            }

            return result;
        }
        #endregion

        #region Private Fields
        private readonly DispatcherTimer _caretTimer;

        private bool _caretBlink;
        #endregion

        #region Private Methods
        private void GenerateTextProperties()
        {
            var formattedText = new FormattedText("x", FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);
            CharSize = formattedText.Measure();
        }

        private void RenderBackground(DrawingContext context)
        {
            foreach (var renderer in BackgroundRenderers)
            {
                renderer.Draw(this, context);
            }
        }

        private void RenderTextBackground(DrawingContext context, VisualLine line)
        {

        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public List<VisualLine> VisualLines { get; private set; }

        private void RenderTextDecoration(DrawingContext context, VisualLine line)
        {

        }

        private int lastLineCount;
        private void GenerateVisualLines()
        {
            VisualLines.Clear();

            uint visualLineNumber = 0;

            for (var i = (int)offset.Y; i < viewport.Height + offset.Y && i < TextDocument.LineCount; i++)
            {
                VisualLines.Add(new VisualLine { DocumentLine = TextDocument.Lines[i], VisualLineNumber = visualLineNumber++ });
            }

            if(TextDocument.LineCount != lastLineCount)
            {
                lastLineCount = TextDocument.LineCount;

                InvalidateMeasure();
                InvalidateScroll.Invoke();
            }
        }

        private void RenderText(DrawingContext context, VisualLine line)
        {
            using (var formattedText = new FormattedText(TextDocument.GetText(line.DocumentLine.Offset, line.DocumentLine.Length), FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal))
            {
                line.RenderedText = formattedText;
                var boundary = VisualLineGeometryBuilder.GetRectsForSegment(this, line).First();

                foreach (var lineTransformer in DocumentLineTransformers)
                {
                    lineTransformer.TransformLine(this, context, boundary, line);
                }

                context.DrawText(Foreground, new Point(0, line.VisualLineNumber * CharSize.Height), formattedText);
            }
        }

        private void RenderCaret(DrawingContext context)
        {
            if (SelectionStart == SelectionEnd)
            {
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
                    var charPos = VisualLineGeometryBuilder.GetTextViewPosition(this, CaretIndex);
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

        private void CaretIndexChanged(int caretIndex)
        {
            if (this.GetVisualParent() != null)
            {
                _caretBlink = true;
                _caretTimer.Stop();
                _caretTimer.Start();
                InvalidateVisual();

                if (caretIndex >= 0)
                {
                    var position = TextDocument.GetLocation(caretIndex);
                    this.BringIntoView(new Rect(position.Column, position.Line - 2, 0, 4));
                }
            }
        }

        private void CaretTimerTick(object sender, EventArgs e)
        {
            _caretBlink = !_caretBlink;
            InvalidateVisual();
        }
        #endregion

        #region Public Methods
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

        public int GetOffsetFromPoint(Point point)
        {
            int result = TextDocument.TextLength;

            if (TextDocument != null)
            {
                var column = Math.Ceiling((point.X / CharSize.Width) + 0.5);
                var line = (int)Math.Ceiling(point.Y / CharSize.Height);

                if (line > 0 && column > 0 && line < TextDocument.LineCount)
                {
                    if (line < VisualLines.Count)
                    {
                        result = TextDocument.GetOffset(VisualLines[line - 1].DocumentLine.LineNumber, (int)column);
                    }
                }
            }

            return result;
        }

        public int GetLine(int caretIndex)
        {
            var line = TextDocument.GetLineByOffset(caretIndex);

            var result = 1;

            if (line != null)
            {
                result = line.LineNumber;
            }

            return result;
        }
        #endregion
    }
}
