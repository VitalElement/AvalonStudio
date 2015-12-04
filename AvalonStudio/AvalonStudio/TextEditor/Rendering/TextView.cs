namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.VisualTree;
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;

    public class TextView : Control
    {
        #region Constructors
        static TextView()
        {
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
        }
        #endregion

        #region Perspex Properties
        public static readonly PerspexProperty<TextWrapping> TextWrappingProperty =
           TextBlock.TextWrappingProperty.AddOwner<TextEditor>();

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
            PerspexProperty.Register<TextView, int>(nameof(CaretIndex));

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionStartProperty =
            TextBox.SelectionStartProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionEndProperty =
            TextBox.SelectionEndProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<Brush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<TextBlock>();
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
        #endregion

        #region Control Overrides
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

        private void RenderTextBackground(DrawingContext context, DocumentLine line)
        {

        }

        private void RenderTextDecoration(DrawingContext context, DocumentLine line)
        {

        }

        private void RenderText(DrawingContext context, DocumentLine line)
        {
            var formattedText = new FormattedText(TextDocument.GetText(line.Offset, line.EndOffset - line.Offset), FontFamily, FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

            foreach (var lineTransformer in DocumentLineTransformers)
            {
                lineTransformer.ColorizeLine(line, formattedText);
            }

            context.DrawText(Brushes.WhiteSmoke, VisualLineGeometryBuilder.GetTextPosition(this, line.Offset).TopLeft, formattedText);
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
                    this.BringIntoView(VisualLineGeometryBuilder.GetTextPosition(this, caretIndex));
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
            int result = -1;

            if (TextDocument != null)
            {
                var column = Math.Ceiling((point.X / CharSize.Width) + 0.5 );
                var line = Math.Ceiling(point.Y / CharSize.Height);

                result = TextDocument.GetOffset((int)line, (int)column);
            }

            return result;
        }

        public int GetLine(int caretIndex)
        {
            return TextDocument.GetLineByOffset(caretIndex).LineNumber;
        }
        #endregion
    }
}
