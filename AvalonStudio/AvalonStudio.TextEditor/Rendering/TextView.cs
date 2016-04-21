namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Shapes;
    using Perspex.Data;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.Utilities;
    using Perspex.VisualTree;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reactive.Linq;
    using Perspex.Input;
    using System.Reactive.Disposables;

    public class TextView : ContentControl, IScrollable
    {
        class WeakEventArgsSubscriber : IWeakSubscriber<EventArgs>
        {
            private readonly Action _onEvent;

            public WeakEventArgsSubscriber(Action onEvent)
            {
                _onEvent = onEvent;
            }

            public void OnEvent(object sender, EventArgs ev)
            {
                _onEvent?.Invoke();
            }
        }

        class WeakCollectionChangedEventArgsSubscriber : IWeakSubscriber<NotifyCollectionChangedEventArgs>
        {
            private readonly Action<NotifyCollectionChangedEventArgs> _onEvent;

            public WeakCollectionChangedEventArgsSubscriber(Action<NotifyCollectionChangedEventArgs> onEvent)
            {
                _onEvent = onEvent;
            }

            public void OnEvent(object sender, NotifyCollectionChangedEventArgs ev)
            {
                _onEvent?.Invoke(ev);
            }
        }

        #region Constructors
        static TextView()
        {
            AffectsMeasure(TextDocumentProperty);
            AffectsRender(DocumentLineTransformersProperty);
        }

        private WeakCollectionChangedEventArgsSubscriber documentLineTransformersChangedSubscriber;
        private WeakCollectionChangedEventArgsSubscriber backgroundRenderersChangedSubscriber;
        private WeakEventArgsSubscriber documentLineTransformerChangedSubscriber;
        private WeakEventArgsSubscriber backgroundRendererChangedSubscriber;
        private WeakEventArgsSubscriber documentTextChangedSubscriber;
        private CompositeDisposable disposables;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _caretTimer.Tick += CaretTimerTick;

            disposables.Add(this.GetObservable(CaretIndexProperty)
                 .Subscribe(CaretIndexChanged));

            disposables.Add(DocumentLineTransformersProperty.Changed.Subscribe((o) =>
            {
                foreach (var item in DocumentLineTransformers)
                {
                    WeakSubscriptionManager.Subscribe(item, nameof(IDocumentLineTransformer.DataChanged), documentLineTransformerChangedSubscriber);
                }

                WeakSubscriptionManager.Subscribe(DocumentLineTransformers, nameof(DocumentLineTransformers.CollectionChanged), documentLineTransformersChangedSubscriber);
            }));

            
            disposables.Add(FontSizeProperty.Changed.Subscribe((o) =>
            {
                GenerateTextProperties();
            }));

            disposables.Add(FontFamilyProperty.Changed.Subscribe((o) =>
            {
                GenerateTextProperties();
            }));

            disposables.Add(FontStyleProperty.Changed.Subscribe((o) =>
            {
                GenerateTextProperties();
            }));

            disposables.Add(FontWeightProperty.Changed.Subscribe((o) =>
            {
                GenerateTextProperties();
            }));
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Dispose();
            _caretTimer.Tick -= CaretTimerTick;
            textSurface = null;
            marginContainer = null;
            TextDocument = null;
            Content = null;
        }

        public TextView()
        {
            disposables = new CompositeDisposable();

            //documentLineTransformersChangedSubscriber = new WeakCollectionChangedEventArgsSubscriber((e) =>
            //{
            //    if (e.NewItems != null)
            //    {
            //        foreach (var item in e.NewItems)
            //        {
            //            WeakSubscriptionManager.Subscribe(item, nameof(IDocumentLineTransformer.DataChanged), documentLineTransformerChangedSubscriber);
            //        }
            //    }
            //});

            //backgroundRenderersChangedSubscriber = new WeakCollectionChangedEventArgsSubscriber((e) =>
            //{
            //    if (e.NewItems != null)
            //    {
            //        foreach (var item in e.NewItems)
            //        {
            //            WeakSubscriptionManager.Subscribe(item, nameof(IBackgroundRenderer.DataChanged), backgroundRendererChangedSubscriber);
            //        }
            //    }
            //});

            //documentLineTransformerChangedSubscriber = new WeakEventArgsSubscriber(() =>
            //{
            //    Invalidate();                
            //});

            //backgroundRendererChangedSubscriber = new WeakEventArgsSubscriber(() =>
            //{
            //    Invalidate();
            //});

            //documentTextChangedSubscriber = new WeakEventArgsSubscriber(() =>
            //{
            //    Invalidate();
            //});

            _caretTimer = new DispatcherTimer();
            _caretTimer.Interval = TimeSpan.FromMilliseconds(500);            

            VisualLines = new List<VisualLine>();
        }

        ~TextView()
        {

        }
        #endregion

        public IVisual TextSurface
        {
            get
            {
                return textSurface;
            }
        }

        public Rect TextSurfaceBounds
        {
            get
            {
                return textSurface.Bounds;
            }
        }

        public TextLocation GetLocation(int offset)
        {
            var documentLocation = TextDocument.GetLocation(offset);

            var result = new TextLocation(documentLocation.Line - firstVisualLine, documentLocation.Column);

            return result;
        }

        public int FindMatchingBracketForward(int startOffset, char open, char close)
        {
            int result = startOffset;

            char currentChar = TextDocument.GetCharAt(startOffset++);

            if (currentChar == open)
            {
                int numOpen = 0;

                while (true)
                {
                    if (startOffset >= TextDocument.TextLength)
                    {
                        break;
                    }

                    currentChar = TextDocument.GetCharAt(startOffset++);

                    if (currentChar == close && numOpen == 0)
                    {
                        result = startOffset - 1;
                        break;
                    }
                    else if (currentChar == open)
                    {
                        numOpen++;
                    }
                    else if (currentChar == close)
                    {
                        numOpen--;
                    }

                    if (startOffset >= TextDocument.TextLength)
                    {
                        break;
                    }
                }
            }

            return result;
        }


        public int FindMatchingBracketBackward(int startOffset, char close, char open)
        {
            int result = startOffset;

            char currentChar = TextDocument.GetCharAt(startOffset--);

            if (currentChar == close)
            {
                int numOpen = 0;

                while (true)
                {
                    if (startOffset < 0)
                    {
                        break;
                    }

                    currentChar = TextDocument.GetCharAt(startOffset--);

                    if (currentChar == open && numOpen == 0)
                    {
                        result = startOffset + 1;
                        break;
                    }
                    else if (currentChar == close)
                    {
                        numOpen++;
                    }
                    else if (currentChar == open)
                    {
                        numOpen--;
                    }

                    if (startOffset >= TextDocument.TextLength)
                    {
                        break;
                    }
                }
            }

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

        public static readonly StyledProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            PerspexProperty.Register<TextView, ObservableCollection<IBackgroundRenderer>>(nameof(BackgroundRenderers), new ObservableCollection<IBackgroundRenderer>());

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IDocumentLineTransformer>> DocumentLineTransformersProperty =
            PerspexProperty.Register<TextView, ObservableCollection<IDocumentLineTransformer>>(nameof(DocumentLineTransformers), new ObservableCollection<IDocumentLineTransformer>());

        public ObservableCollection<IDocumentLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
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

        public static readonly StyledProperty<TextDocument> TextDocumentProperty =
            PerspexProperty.Register<TextView, TextDocument>(nameof(TextDocument));

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); InvalidateMeasure(); }
        }

        public static readonly StyledProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>(nameof(BeforeTextChangedCommand));

        public System.Windows.Input.ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly StyledProperty<System.Windows.Input.ICommand> TextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>(nameof(TextChangedCommand));

        public System.Windows.Input.ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly StyledProperty<int> CaretIndexProperty =
            PerspexProperty.Register<TextView, int>(nameof(CaretIndex), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay);

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<TextViewMargin>> MarginsProperty =
            PerspexProperty.Register<TextView, ObservableCollection<TextViewMargin>>(nameof(Margins), new ObservableCollection<TextViewMargin>());

        public ObservableCollection<TextViewMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionStartProperty =
            PerspexProperty.Register<TextView, int>(nameof(SelectionStart));

        public static readonly PerspexProperty<int> SelectionEndProperty =
            PerspexProperty.Register<TextView, int>(nameof(SelectionEnd));

        public static readonly PerspexProperty<IBrush> ForegoundProperty =
            TextBlock.ForegroundProperty.AddOwner<TextView>();

        public IBrush Foreground
        {
            get { return GetValue(ForegoundProperty); }
            set { SetValue(ForegoundProperty, value); }
        }

        public static readonly PerspexProperty<IBrush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<TextView>();

        public IBrush Background
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
        private Size charSize;
        public Size CharSize
        {
            get { return charSize; }
            set { charSize = value; }
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

                Invalidate();
            }
        }

        private Size viewport;
        public Size Viewport
        {
            get { return viewport; }
        }
        #endregion

        #region Control Overrides
        private StackPanel marginContainer;
        private Rectangle textSurface;
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            marginContainer = e.NameScope.Find<StackPanel>("marginContainer");
            textSurface = e.NameScope.Find<Rectangle>("textSurface");
        }

        public void InstallMargin(Control margin)
        {
            if (marginContainer != null)
            {
                marginContainer.Children.Add(margin);
            }
        }

        public void Invalidate()
        {
            invalidateVisualLines = true;
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            if (TextDocument != null)
            {
                //if (invalidateVisualLines)
                {
                    GenerateVisualLines(context);                
                }

                // Render background layer.
                RenderBackground(context);

                foreach (var line in VisualLines)
                {
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
            }

            base.ArrangeOverride(finalSize);

            return result;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size result = availableSize;

            if (TextDocument != null)
            {
                GenerateTextProperties();

                result = new Size(0, TextDocument.LineCount * CharSize.Height);
            }

            base.MeasureOverride(availableSize);

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
            using (var formattedText = new FormattedText("x", FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal))
            {
                CharSize = formattedText.Measure();
            }
        }

        private void RenderBackground(DrawingContext context)
        {
            foreach (var renderer in BackgroundRenderers)
            {
                renderer.Draw(this, context);

                foreach (var line in VisualLines)
                {
                    if (!line.DocumentLine.IsDeleted)
                    {
                        renderer.TransformLine(this, context, line);
                    }
                }
            }
        }

        private void RenderTextBackground(DrawingContext context, VisualLine line)
        {
            foreach (var renderer in BackgroundRenderers)
            {
                renderer.TransformLine(this, context, line);
            }
        }

        private IEnumerable<Point> CreatePoints(Point start, Point end, double offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public List<VisualLine> VisualLines { get; private set; }

        public Size ScrollSize
        {
            get
            {
                return new Size(Bounds.Width, 2);
            }
        }

        public Size PageScrollSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private void RenderTextDecoration(DrawingContext context, VisualLine line)
        {

        }

        private bool invalidateVisualLines = true;
        private int lastLineCount;
        private void GenerateVisualLines(DrawingContext context)
        {
            VisualLines.Clear();

            uint visualLineNumber = 0;

            for (var i = (int)offset.Y; i < viewport.Height + offset.Y && i < TextDocument.LineCount && i >= 0; i++)
            {
                var line = new VisualLine { DocumentLine = TextDocument.Lines[i], VisualLineNumber = visualLineNumber++ };
                GenerateText(line);
                VisualLines.Add(line);
            }

            if (TextDocument.LineCount != lastLineCount)
            {
                lastLineCount = TextDocument.LineCount;

                InvalidateMeasure();
                InvalidateScroll?.Invoke();
            }

            invalidateVisualLines = false;
        }

        private void GenerateText(VisualLine line)
        {
            if (!line.DocumentLine.IsDeleted)
            {
                var formattedText = new FormattedText(TextDocument.GetText(line.DocumentLine.Offset, line.DocumentLine.Length), FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

                line.RenderedText = formattedText;

                foreach (var lineTransformer in DocumentLineTransformers)
                {
                    lineTransformer.TransformLine(this, line);
                }
            }
        }

        private void RenderText(DrawingContext context, VisualLine line)
        {
            context.DrawText(Foreground, new Point(TextSurfaceBounds.X, line.VisualLineNumber * CharSize.Height), line.RenderedText);
            line.RenderedText.Dispose();
        }

        private void RenderCaret(DrawingContext context)
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

            if (_caretBlink && CaretIndex != -1)
            {
                if (CaretIndex > TextDocument.TextLength)
                {
                    CaretIndex = TextDocument.TextLength;
                }

                var charPos = VisualLineGeometryBuilder.GetTextViewPosition(this, CaretIndex);
                var x = Math.Floor(charPos.X) + 0.5;
                var y = Math.Floor(charPos.Y) + 0.5;
                var b = Math.Ceiling(charPos.Bottom) - 0.5;

                context.DrawLine(
                    new Pen(caretBrush, 1),
                    new Point(charPos.TopLeft.X, charPos.TopLeft.Y),
                    new Point(charPos.TopLeft.X, charPos.BottomLeft.Y));
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

                if (caretIndex >= 0 && TextDocument != null)
                {
                    var position = TextDocument.GetLocation(caretIndex);
                    ScrollToLine(position.Line, 0.1);
                }
            }
        }

        public void ScrollToLine(int line, double borderSizePc = 0.5)
        {
            var offset = (line - (Viewport.Height * borderSizePc));

            if (offset < 0)
            {
                offset = 0;
            }

            this.BringIntoView(new Rect(1, offset, 0, 1));

            offset = (line + (Viewport.Height * borderSizePc));

            if (offset >= 0)
            {
                this.BringIntoView(new Rect(1, offset, 0, 1));
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

        public void PageUp()
        {
            if (VisualLines.First().DocumentLine != null)
            {
                ScrollToLine(VisualLines.First().DocumentLine.LineNumber, 0.75);
            }
        }

        public void PageDown()
        {
            if (VisualLines.Last().DocumentLine != null)
            {
                ScrollToLine(VisualLines.Last().DocumentLine.LineNumber, 0.75);
            }
        }

        public int GetOffsetFromPoint(Point point)
        {
            int result = -1;

            if (TextDocument != null)
            {
                var column = Math.Ceiling((point.X / CharSize.Width) + 0.5);
                var line = (int)Math.Ceiling(point.Y / CharSize.Height);

                if (line > 0 && column > 0 && line-1 < VisualLines.Count)
                {
                    if (line < VisualLines.Count && !VisualLines[line - 1].DocumentLine.IsDeleted && VisualLines[line - 1].DocumentLine.LineNumber < TextDocument.LineCount)
                    {
                        result = TextDocument.GetOffset(VisualLines[line - 1].DocumentLine.LineNumber, (int)column);
                    }
                    else
                    {
                        // Invalid
                        result = -1;
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
