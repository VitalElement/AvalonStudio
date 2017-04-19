namespace AvalonStudio.TextEditor.Rendering
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Primitives;
    using Avalonia.Controls.Shapes;
    using Avalonia.Data;
    using Avalonia.Input;
    using Avalonia.Layout;
    using Avalonia.LogicalTree;
    using Avalonia.Media;
    using Avalonia.Threading;
    using Avalonia.Utilities;
    using Avalonia.VisualTree;
    using AvalonStudio.TextEditor.Document;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Reactive.Disposables;
    using System.Reactive.Linq;
    using System.Windows.Input;

    public class TextView : TemplatedControl, ILogicalScrollable
    {
        public static readonly StyledProperty<object> ContentProperty = ContentControl.ContentProperty.AddOwner<TextView>();

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private int lastLineScrolledTo = -1;

        public IVisual TextSurface
        {
            get { return textSurface; }
        }

        public Rect TextSurfaceBounds
        {
            get { return textSurface.Bounds; }
        }

        public TextLocation GetLocation(int offset)
        {
            if (offset > TextDocument.TextLength)
            {
                offset = TextDocument.TextLength;
            }

            var documentLocation = TextDocument.GetLocation(offset);

            var result = new TextLocation(documentLocation.Line - firstVisualLine, documentLocation.Column);

            return result;
        }

        public int FindMatchingBracketForward(int startOffset, char open, char close)
        {
            var result = startOffset;

            var currentChar = TextDocument.GetCharAt(startOffset++);

            if (currentChar == open)
            {
                var numOpen = 0;

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
                    if (currentChar == open)
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
            var result = startOffset;

            var currentChar = TextDocument.GetCharAt(startOffset--);

            if (currentChar == close)
            {
                var numOpen = 0;

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
                    if (currentChar == close)
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

        private class WeakEventArgsSubscriber : IWeakSubscriber<EventArgs>
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

        private class WeakCollectionChangedEventArgsSubscriber : IWeakSubscriber<NotifyCollectionChangedEventArgs>
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
            AffectsArrange(OffsetProperty);
            AffectsMeasure(TextDocumentProperty);
            AffectsRender(OffsetProperty);
            AffectsRender(DocumentLineTransformersProperty);
        }

        private WeakCollectionChangedEventArgsSubscriber documentLineTransformersChangedSubscriber;
        private WeakCollectionChangedEventArgsSubscriber backgroundRenderersChangedSubscriber;
        private WeakEventArgsSubscriber documentLineTransformerChangedSubscriber;
        private WeakEventArgsSubscriber backgroundRendererChangedSubscriber;
        private WeakEventArgsSubscriber documentTextChangedSubscriber;
        private IDisposable collectionChangedDisposable;
        private readonly CompositeDisposable disposables;

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _caretTimer.Tick += CaretTimerTick;

            disposables.Add(this.GetObservable(CaretIndexProperty)
                .Subscribe(CaretIndexChanged));

            disposables.Add(DocumentLineTransformersProperty.Changed.Subscribe(o =>
            {
                foreach (var item in DocumentLineTransformers)
                {
                    WeakSubscriptionManager.Subscribe(item, nameof(IDocumentLineTransformer.DataChanged),
                        documentLineTransformerChangedSubscriber);
                }

                WeakSubscriptionManager.Subscribe(DocumentLineTransformers, nameof(DocumentLineTransformers.CollectionChanged),
                    documentLineTransformersChangedSubscriber);
            }));

            disposables.Add(FontSizeProperty.Changed.Subscribe(o =>
            {
                GenerateTextProperties();

                Invalidate();
            }));

            disposables.Add(FontFamilyProperty.Changed.Subscribe(o => { GenerateTextProperties(); }));

            disposables.Add(FontStyleProperty.Changed.Subscribe(o => { GenerateTextProperties(); }));

            disposables.Add(FontWeightProperty.Changed.Subscribe(o => { GenerateTextProperties(); }));
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            Margins.Clear();
            disposables.Dispose();
            collectionChangedDisposable.Dispose();
            _caretTimer.Tick -= CaretTimerTick;
            textSurface = null;
            marginContainer = null;
            TextDocument = null;
        }

        public TextView()
        {
            disposables = new CompositeDisposable();

            disposables.Add(MarginProperty.Changed.AddClassHandler<TextView>((s, v) =>
            {
                if (collectionChangedDisposable != null)
                {
                    collectionChangedDisposable.Dispose();
                }

                collectionChangedDisposable = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(s.Margins, "CollectionChanged").Subscribe(o =>
                {
                    switch (o.EventArgs.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            foreach (var newItem in o.EventArgs.NewItems)
                            {
                                if (newItem as ILogical != null)
                                {
                                    s.LogicalChildren.Add(newItem as ILogical);
                                    InstallMargin(newItem as Control);
                                }
                            }
                            break;

                        case NotifyCollectionChangedAction.Remove:
                            foreach (var oldItem in o.EventArgs.OldItems)
                            {
                                if (oldItem as ILogical != null)
                                {
                                    s.LogicalChildren.Remove(oldItem as ILogical);
                                    UninstallMargin(oldItem as Control);
                                }
                            }
                            break;
                    }
                });
            }));

            _caretTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };

            VisualLines = new List<VisualLine>();
        }

        ~TextView()
        {
            foreach (var visualLine in VisualLines)
            {
                visualLine.RenderedText?.Dispose();
            }

            VisualLines.Clear();
        }

        #endregion Constructors

        #region Avalonia Properties

        public static readonly AvaloniaProperty<TextWrapping> TextWrappingProperty =
            TextBlock.TextWrappingProperty.AddOwner<TextView>();

        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            AvaloniaProperty.Register<TextView, ObservableCollection<IBackgroundRenderer>>(nameof(BackgroundRenderers),
                new ObservableCollection<IBackgroundRenderer>());

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<IDocumentLineTransformer>> DocumentLineTransformersProperty
            =
            AvaloniaProperty.Register<TextView, ObservableCollection<IDocumentLineTransformer>>(
                nameof(DocumentLineTransformers), new ObservableCollection<IDocumentLineTransformer>());

        public ObservableCollection<IDocumentLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
        }

        public static readonly AvaloniaProperty<string> FontFamilyProperty =
            TextEditor.FontFamilyProperty.AddOwner<TextView>();

        public string FontFamily
        {
            get { return GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public static readonly AvaloniaProperty<double> FontSizeProperty = TextEditor.FontSizeProperty.AddOwner<TextView>();

        public double FontSize
        {
            get { return GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly AvaloniaProperty<bool> AcceptsReturnProperty =
            AvaloniaProperty.Register<TextView, bool>(nameof(AcceptsReturn), true);

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly AvaloniaProperty<bool> AcceptsTabProperty =
            AvaloniaProperty.Register<TextView, bool>(nameof(AcceptsTab));

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly StyledProperty<TextDocument> TextDocumentProperty =
            AvaloniaProperty.Register<TextView, TextDocument>(nameof(TextDocument));

        public TextDocument TextDocument
        {
            get
            {
                return GetValue(TextDocumentProperty);
            }
            set
            {
                SetValue(TextDocumentProperty, value);
                InvalidateMeasure();
            }
        }

        public static readonly StyledProperty<ICommand> BeforeTextChangedCommandProperty =
            AvaloniaProperty.Register<TextView, ICommand>(nameof(BeforeTextChangedCommand));

        public ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly StyledProperty<ICommand> TextChangedCommandProperty =
            AvaloniaProperty.Register<TextView, ICommand>(nameof(TextChangedCommand));

        public ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly StyledProperty<int> CaretIndexProperty =
            AvaloniaProperty.Register<TextView, int>(nameof(CaretIndex), 0, defaultBindingMode: BindingMode.TwoWay);

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<TextViewMargin>> MarginsProperty =
            AvaloniaProperty.Register<TextView, ObservableCollection<TextViewMargin>>(nameof(Margins),
                new ObservableCollection<TextViewMargin>());

        public ObservableCollection<TextViewMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        public static readonly AvaloniaProperty<int> SelectionStartProperty =
            AvaloniaProperty.Register<TextView, int>(nameof(SelectionStart));

        public static readonly AvaloniaProperty<int> SelectionEndProperty =
            AvaloniaProperty.Register<TextView, int>(nameof(SelectionEnd));

        public static readonly AvaloniaProperty<IBrush> ForegoundProperty =
            TextBlock.ForegroundProperty.AddOwner<TextView>();

        public IBrush Foreground
        {
            get { return GetValue(ForegoundProperty); }
            set { SetValue(ForegoundProperty, value); }
        }

        /// <summary>
        ///     Defines the <see cref="Offset" /> property.
        /// </summary>
        public static readonly DirectProperty<TextView, Vector> OffsetProperty =
            AvaloniaProperty.RegisterDirect<TextView, Vector>(
                nameof(Offset),
                o => o.Offset,
                (o, v) => o.Offset = v);

        public static readonly AvaloniaProperty<IBrush> BackgroundProperty =
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

        #endregion Avalonia Properties

        #region Properties

        public Size CharSize { get; set; }

        public Action InvalidateScroll { get; set; }

        public Size Extent { get; private set; }

        private int firstVisualLine;

        private Vector offset;

        public Vector Offset
        {
            get
            {
                return offset;
            }
            set
            {
                if ((value.Y != offset.Y || value.X != offset.X) && firstVisualLine != (int)value.Y)
                {
                    firstVisualLine = (int)value.Y;

                    SetAndRaise(OffsetProperty, ref offset, value);
                }
            }
        }

        public Size Viewport { get; private set; }

        #endregion Properties

        #region Control Overrides

        private StackPanel marginContainer;
        private Rectangle textSurface;
        private ContentControl contentPresenter;

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            marginContainer = e.NameScope.Find<StackPanel>("marginsPanel");
            textSurface = e.NameScope.Find<Rectangle>("textSurface");
            contentPresenter = e.NameScope.Find<ContentControl>("contentPresenter");

            foreach (var margin in Margins)
            {
                if (margin is ILogical)
                {
                    LogicalChildren.Add(margin as ILogical);
                }

                InstallMargin(margin);
            }

            collectionChangedDisposable = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(Margins, "CollectionChanged").Subscribe(o =>
            {
                switch (o.EventArgs.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var newItem in o.EventArgs.NewItems)
                        {
                            if (newItem as ILogical != null)
                            {
                                LogicalChildren.Add(newItem as ILogical);
                                InstallMargin(newItem as Control);
                            }
                        }
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var oldItem in o.EventArgs.OldItems)
                        {
                            if (oldItem as ILogical != null)
                            {
                                LogicalChildren.Remove(oldItem as ILogical);
                                UninstallMargin(oldItem as Control);
                            }
                        }
                        break;
                }
            });
        }

        public void InstallMargin(Control margin)
        {
            if (marginContainer != null)
            {
                marginContainer.Children.Add(margin);
            }
        }

        public void UninstallMargin(Control margin)
        {
            if (marginContainer != null)
            {
                marginContainer.Children.Remove(margin);
            }
        }

        public void Invalidate()
        {
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            if (TextDocument != null && !TextDocument.IsInUpdate)
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

        private int LogicalScrollSize
        {
            get
            {
                if (TextDocument != null)
                {
                    return TextDocument.LineCount + 20;
                }
                return 20;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TextDocument != null)
            {
                GenerateTextProperties();

                Viewport = new Size(finalSize.Width, finalSize.Height / CharSize.Height);
                Extent = new Size(finalSize.Width, LogicalScrollSize);

                InvalidateScroll.Invoke();
            }

            var result = finalSize;
            var arrangeOffset = new Vector(Math.Floor(Offset.X) * CharSize.Width, Math.Floor(Offset.Y) * CharSize.Height);

            result = base.ArrangeOverride(new Size(result.Width, result.Height + arrangeOffset.Y));

            var child = contentPresenter as ILayoutable;

            if (child != null)
            {
                child.Arrange(new Rect((Point)(-arrangeOffset), child.DesiredSize));
            }

            return result;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var result = availableSize;

            if (TextDocument != null)
            {
                GenerateTextProperties();

                result = new Size(availableSize.Width, LogicalScrollSize * CharSize.Height);

                base.MeasureOverride(result);
            }

            return result;
        }

        #endregion Control Overrides

        #region Private Fields

        private readonly DispatcherTimer _caretTimer;

        private bool _caretBlink;

        #endregion Private Fields

        #region Private Methods

        public void GenerateTextProperties()
        {
            using (
                var formattedText = new FormattedText("x", FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left,
                    FontWeight.Normal))
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
            for (var i = 0; i < count; i++)
            {
                yield return new Point(start.X + (i * offset), start.Y - ((i + 1) % 2 == 0 ? offset : 0));
            }
        }

        public List<VisualLine> VisualLines { get; }

        public Size ScrollSize
        {
            get { return new Size(Bounds.Width, 2); }
        }

        public Size PageScrollSize
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsLogicalScrollEnabled
        {
            get { return true; }
        }

        private void RenderTextDecoration(DrawingContext context, VisualLine line)
        {
        }

        private int lastLineCount;

        private void GenerateVisualLines(DrawingContext context)
        {
            VisualLines.Clear();

            uint visualLineNumber = 0;

            for (var i = (int)offset.Y; i < Viewport.Height + offset.Y && i < TextDocument.LineCount && i >= 0; i++)
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
        }

        private void GenerateText(VisualLine line)
        {
            if (!line.DocumentLine.IsDeleted)
            {
                var formattedText = new FormattedText(TextDocument.GetText(line.DocumentLine.Offset, line.DocumentLine.Length),
                    FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

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
        }

        private void RenderCaret(DrawingContext context)
        {
            var backgroundColor = (((Control)TemplatedParent).GetValue(BackgroundProperty) as SolidColorBrush)?.Color;
            var caretBrush = Brushes.Black;

            if (backgroundColor.HasValue)
            {
                var red = (byte)~backgroundColor.Value.R;
                var green = (byte)~backgroundColor.Value.G;
                var blue = (byte)~backgroundColor.Value.B;

                caretBrush = new SolidColorBrush(Color.FromRgb(red, green, blue));
            }

            if (_caretBlink && CaretIndex != -1)
            {
                if (CaretIndex > TextDocument.TextLength)
                {
                    CaretIndex = TextDocument.TextLength;
                }

                var charPos = VisualLineGeometryBuilder.GetViewPortPosition(this, CaretIndex);
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

                    if (lastLineScrolledTo != position.Line)
                    {
                        ScrollToLine(position.Line, 0.1);

                        lastLineScrolledTo = position.Line;
                    }
                }
            }
        }

        public void ScrollToLine(int line, double borderSizePc = 0.5)
        {
            var offset = line - (Viewport.Height * borderSizePc);

            if (offset < 0)
            {
                offset = 0;
            }

            this.BringIntoView(new Rect(1, offset, 0, 1));

            offset = line + (Viewport.Height * borderSizePc);

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

        #endregion Private Methods

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

        public int GetCaretIndex(FormattedText lineText, Point point)
        {
            var hit = lineText.HitTestPoint(new Point(point.X, 0));

            return hit.TextPosition + (hit.IsTrailing ? 1 : 0) + 1;
        }

        public int GetOffsetFromPoint(Point point)
        {
            var result = -1;

            if (TextDocument != null)
            {
                var line = (int)Math.Ceiling(point.Y / CharSize.Height);

                if (line > 0 && line - 1 < VisualLines.Count)
                {
                    var column = GetCaretIndex(VisualLines[line - 1].RenderedText, point);

                    if (line - 1 < VisualLines.Count && !VisualLines[line - 1].DocumentLine.IsDeleted && VisualLines[line - 1].DocumentLine.LineNumber - 1 < TextDocument.LineCount)
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

        public bool BringIntoView(IControl target, Rect targetRect)
        {
            var result = false;

            if (firstVisualLine > targetRect.Y)
            {
                Offset = Offset.WithY(targetRect.Y);
                result = true;
            }

            if (firstVisualLine + Viewport.Height < targetRect.Y)
            {
                Offset = Offset.WithY(targetRect.Y - Viewport.Height);
                result = true;
            }

            return result;
        }

        public IControl GetControlInDirection(NavigationDirection direction, IControl from)
        {
            return null;
        }

        #endregion Public Methods
    }
}