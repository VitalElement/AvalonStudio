namespace AvalonStudio.TextEditor.Rendering
{
    using Document;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Input;
    using Perspex.Input.Platform;
    using Perspex.Media;
    using Perspex.Threading;
    using Perspex.VisualTree;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using Perspex.Interactivity;
    using Perspex.Controls.Primitives;

    public class TextView : Control
    {
        #region Constructors
        static TextView()
        {
            TextChangedDelayProperty.Changed.AddClassHandler<TextView>((s, v) => s.textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)v.NewValue));
        }

        public TextView(TextEditor editor)
        {
            this.editor = editor;

            textChangedDelayTimer = new DispatcherTimer();
            textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            textChangedDelayTimer.Tick += TextChangedDelayTimer_Tick;
            textChangedDelayTimer.Stop();

            _caretTimer = new DispatcherTimer();
            _caretTimer.Interval = TimeSpan.FromMilliseconds(500);
            _caretTimer.Tick += CaretTimerTick;

            var canScrollHorizontally = GetObservable(AcceptsReturnProperty)
                .Select(x => !x);

            Bind(
                ScrollViewer.CanScrollHorizontallyProperty,
                canScrollHorizontally,
                BindingPriority.Style);

            var horizontalScrollBarVisibility = GetObservable(AcceptsReturnProperty)
                .Select(x => x ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);

            Bind(
                ScrollViewer.HorizontalScrollBarVisibilityProperty,
                horizontalScrollBarVisibility,
                BindingPriority.Style);

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

        public static readonly PerspexProperty<bool> AcceptsReturnProperty =
            PerspexProperty.Register<TextView, bool>("AcceptsReturn", defaultValue: true);

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsTabProperty =
            PerspexProperty.Register<TextView, bool>("AcceptsTab");

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly PerspexProperty<TextDocument> TextDocumentProperty =
            PerspexProperty.Register<TextView, TextDocument>("TextDocument");

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>("BeforeTextChangedCommand");

        public System.Windows.Input.ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> TextChangedCommandProperty =
            PerspexProperty.Register<TextView, System.Windows.Input.ICommand>("TextChangedCommand");

        public System.Windows.Input.ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<int> TextChangedDelayProperty =
            PerspexProperty.Register<TextView, int>("TextChangedDelay");

        public int TextChangedDelay
        {
            get { return GetValue(TextChangedDelayProperty); }
            set { SetValue(TextChangedDelayProperty, value); textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, value); }
        }

        public static readonly PerspexProperty<int> CaretIndexProperty =
            TextBox.CaretIndexProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionStartProperty =
            TextBox.SelectionStartProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<int> SelectionEndProperty =
            TextBox.SelectionEndProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<Brush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<TextBlock>();

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

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            if (e.Source == this)
            {
                var point = e.GetPosition(this);
                var index = CaretIndex = GetOffsetFromPoint(point);
                var text = TextDocument;

                switch (e.ClickCount)
                {
                    case 1:
                        SelectionStart = SelectionEnd = index;
                        break;
                    case 2:
                        //if (!StringUtils.IsStartOfWord(text, index))
                        //{
                        //    SelectionStart = StringUtils.PreviousWord(text, index, false);
                        //}

                        //SelectionEnd = StringUtils.NextWord(text, index, false);
                        break;
                    case 3:
                        SelectionStart = 0;
                        SelectionEnd = text.TextLength;
                        break;
                }

                e.Device.Capture(this);
                e.Handled = true;
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (e.Device.Captured == this)
            {
                var point = e.GetPosition(this);
                CaretIndex = SelectionEnd = GetOffsetFromPoint(point);
            }
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (e.Device.Captured == this)
            {
                e.Device.Capture(null);
            }
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            ShowCaret();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            SelectionStart = 0;
            SelectionEnd = 0;
            HideCaret();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            HandleTextInput(e.Text);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            int caretIndex = CaretIndex;
            bool movement = false;
            bool handled = true;
            var modifiers = e.Modifiers;

            switch (e.Key)
            {
                case Key.A:
                    if (modifiers == InputModifiers.Control)
                    {
                        SelectAll();
                    }

                    break;
                case Key.C:
                    if (modifiers == InputModifiers.Control)
                    {
                        Copy();
                    }

                    break;
                case Key.V:
                    if (modifiers == InputModifiers.Control)
                    {
                        Paste();
                    }

                    break;

                case Key.Left:
                    MoveHorizontal(-1, modifiers);
                    movement = true;
                    break;

                case Key.Right:
                    MoveHorizontal(1, modifiers);
                    movement = true;
                    break;

                case Key.Up:
                    MoveVertical(-1, modifiers);
                    movement = true;
                    break;

                case Key.Down:
                    MoveVertical(1, modifiers);
                    movement = true;
                    break;

                case Key.Home:
                    MoveHome(modifiers);
                    movement = true;
                    break;

                case Key.End:
                    MoveEnd(modifiers);
                    movement = true;
                    break;

                case Key.Back:
                    if (!DeleteSelection() && CaretIndex > 0)
                    {
                        TextDocument.Remove(caretIndex - 1, 1);
                        --CaretIndex;
                    }

                    break;

                case Key.Delete:
                    if (!DeleteSelection() && caretIndex < TextDocument.TextLength)
                    {
                        TextDocument.Remove(caretIndex, 1);
                    }

                    break;

                case Key.Enter:
                    if (AcceptsReturn)
                    {
                        HandleTextInput("\r\n");
                    }

                    break;

                case Key.Tab:
                    if (AcceptsTab)
                    {
                        HandleTextInput("\t");
                    }
                    else
                    {
                        base.OnKeyDown(e);
                        handled = false;
                    }

                    break;
            }

            if (movement && ((modifiers & InputModifiers.Shift) != 0))
            {
                SelectionEnd = CaretIndex;
            }
            else if (movement)
            {
                SelectionStart = SelectionEnd = CaretIndex;
            }

            if (handled)
            {
                e.Handled = true;
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
        private readonly DispatcherTimer textChangedDelayTimer;

        private readonly DispatcherTimer _caretTimer;

        private bool _caretBlink;

        private IObservable<bool> _canScrollHorizontally;

        private TextEditor editor;
        #endregion


        #region Private Methods
        private void GenerateTextProperties()
        {
            var formattedText = new FormattedText("x", editor.FontFamily, editor.FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);
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
            var formattedText = new FormattedText(TextDocument.GetText(line.Offset, line.EndOffset - line.Offset), editor.FontFamily, editor.FontSize, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal);

            foreach (var lineTransformer in DocumentLineTransformers)
            {
                lineTransformer.ColorizeLine(line, formattedText);
            }

            context.DrawText(Brushes.WhiteSmoke, VisualLineGeometryBuilder.GetTextPosition(this, line.Offset).TopLeft, formattedText);
        }

        private void TextChangedDelayTimer_Tick(object sender, EventArgs e)
        {
            textChangedDelayTimer.Stop();

            if (TextChangedCommand != null)
            {
                TextChangedCommand.Execute(null);
            }
        }

        private void ShowCaret()
        {
            _caretBlink = true;
            _caretTimer.Start();
            InvalidateVisual();
        }

        private void HideCaret()
        {
            _caretBlink = false;
            _caretTimer.Stop();
            InvalidateVisual();
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

        private void SelectAll()
        {
            SelectionStart = 0;
            SelectionEnd = TextDocument.TextLength;
        }

        private bool DeleteSelection()
        {
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;

            if (selectionStart != selectionEnd)
            {
                var start = Math.Min(selectionStart, selectionEnd);
                var end = Math.Max(selectionStart, selectionEnd);
                TextDocument.Remove(start, end);
                SelectionStart = SelectionEnd = CaretIndex = start;
                return true;
            }
            else
            {
                return false;
            }
        }

        private string GetSelection()
        {
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;
            var start = Math.Min(selectionStart, selectionEnd);
            var end = Math.Max(selectionStart, selectionEnd);

            if (start == end || (TextDocument?.TextLength ?? 0) < end)
            {
                return "";
            }

            return TextDocument.GetText(start, end - start);
        }

        private void HandleTextInput(string input)
        {
            if (BeforeTextChangedCommand != null)
            {
                BeforeTextChangedCommand.Execute(null);
            }

            //string text = TextDocument ?? string.Empty;
            int caretIndex = CaretIndex;

            if (!string.IsNullOrEmpty(input))
            {
                DeleteSelection();
                caretIndex = CaretIndex;
                TextDocument.Insert(caretIndex, input);
                CaretIndex += input.Length;
                SelectionStart = SelectionEnd = CaretIndex;
            }

            textChangedDelayTimer.Stop();
            textChangedDelayTimer.Start();
        }

        private void MoveHorizontal(int count, InputModifiers modifiers)
        {
            var caretIndex = CaretIndex;

            //if ((modifiers & InputModifiers.Control) != 0)
            //{
            //    if (count > 0)
            //    {
            //        // count = StringUtils.NextWord(text, caretIndex, false) - caretIndex;
            //    }
            //    else
            //    {
            //        //count = StringUtils.PreviousWord(text, caretIndex, false) - caretIndex;
            //    }
            //}

            CaretIndex = caretIndex += count;
        }

        private void MoveVertical(int count, InputModifiers modifiers)
        {
            var caretIndex = CaretIndex;
            var lineIndex = GetLine(caretIndex) + count;

            if (lineIndex > 0 && lineIndex <= TextDocument.LineCount)
            {
                var line = TextDocument.Lines[lineIndex - 1];

                var rect = VisualLineGeometryBuilder.GetTextPosition(this, caretIndex);
                var y = count < 0 ? rect.Y : rect.Bottom;
                var point = new Point(rect.X, y + (count * (CharSize.Height / 2)));

                CaretIndex = GetOffsetFromPoint(point);
            }
        }

        private void MoveHome(InputModifiers modifiers)
        {
            //var text = TextDocument ?? string.Empty;
            //var caretIndex = CaretIndex;

            //if ((modifiers & InputModifiers.Control) != 0)
            //{
            //    caretIndex = 0;
            //}
            //else
            //{
            //    var lines = textView.FormattedText.GetLines();
            //    var pos = 0;

            //    foreach (var line in lines)
            //    {
            //        if (pos + line.Length > caretIndex || pos + line.Length == text.Length)
            //        {
            //            break;
            //        }

            //        pos += line.Length;
            //    }

            //    caretIndex = pos;
            //}

            //CaretIndex = caretIndex;
        }

        private void MoveEnd(InputModifiers modifiers)
        {
            //var text = TextDocument ?? string.Empty;
            //var caretIndex = CaretIndex;

            //if ((modifiers & InputModifiers.Control) != 0)
            //{
            //    caretIndex = text.Length;
            //}
            //else
            //{
            //    var lines = textView.FormattedText.GetLines();
            //    var pos = 0;

            //    foreach (var line in lines)
            //    {
            //        pos += line.Length;

            //        if (pos > caretIndex)
            //        {
            //            if (pos < text.Length)
            //            {
            //                --pos;
            //            }

            //            break;
            //        }
            //    }

            //    caretIndex = pos;
            //}

            //CaretIndex = caretIndex;
        }

        private async void Copy()
        {
            await ((IClipboard)PerspexLocator.Current.GetService(typeof(IClipboard)))
                .SetTextAsync(GetSelection());
        }

        private async void Paste()
        {
            var text = await ((IClipboard)PerspexLocator.Current.GetService(typeof(IClipboard))).GetTextAsync();
            if (text == null)
            {
                return;
            }

            HandleTextInput(text);
        }
        #endregion

        #region Public Methods
        public int GetOffsetFromPoint(Point point)
        {
            int result = -1;

            if (TextDocument != null)
            {
                var column = Math.Ceiling(point.X / CharSize.Width);
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
