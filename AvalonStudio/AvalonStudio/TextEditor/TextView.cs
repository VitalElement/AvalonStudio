namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Presenters;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Input;
    using Perspex.Input.Platform;
    using Perspex.Interactivity;
    using Perspex.Media;
    using Perspex.Styling;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;

    //class TextView : Control
    //{
    //    public TextView()
    //    {
    //        Text = "Not Clicked";

    //    }

    //    public static readonly PerspexProperty<string> TextProperty = PerspexProperty.Register<TextView, string>("Text", null, false, BindingMode.TwoWay);

    //    protected override void OnPointerPressed(PointerPressEventArgs e)
    //    {

    //    }

    //    protected override void OnPointerMoved(PointerEventArgs e)
    //    {
    //        double y = e.Device.GetPosition(this).Y;
    //        y /= lineHeight;            
    //        InvalidateVisual();
    //    }

    //    double lineHeight = 20;
    //    public override void Render(IDrawingContext context)
    //    {       
    //        var textSize = new FormattedText("X", "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size };
    //        lineHeight = textSize.Measure().Height;

    //        if (Text != null)
    //        {
    //            var text = new FormattedText(Text, "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size };

    //            var renderSize = new Rect(Bounds.Size);

    //            context.FillRectangle(Brush.Parse("#1e1e1e"), renderSize);

    //            var lines = (int)(Bounds.Height / lineHeight);

    //            context.FillRectangle(Brush.Parse("#242424"), new Rect(Bounds.X + 14, (SelectedLine * lineHeight), Bounds.Width, lineHeight), 4);
    //            context.FillRectangle(Brush.Parse("#0f0f0f"), new Rect(Bounds.X + 14 + 2, (SelectedLine * lineHeight) + 2, Bounds.Width - 4, lineHeight - 4), 4);

    //            for (int i = 0; i < lines; i++)
    //            {
    //                context.DrawText(Brushes.WhiteSmoke, new Point(0, 4 + (i * lineHeight)), new FormattedText((i + 1).ToString(), "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size });
    //            }

    //            text.SetForegroundBrush(Brushes.Red, 50, 20);
    //            text.SetForegroundBrush(Brushes.Pink, 10, 5);
    //            text.SetForegroundBrush(Brushes.Green, 200, 500);

    //            context.DrawText(Brushes.White, new Point(18, 4), text);
    //        }            
    //    }

    //    private int selectedLine;
    //    public int SelectedLine
    //    {
    //        get { return selectedLine; }
    //        set { selectedLine = value; }
    //    }

    //    public string Text
    //    {
    //        get { return GetValue(TextProperty); }
    //        set { SetValue(TextProperty, value); }
    //    }


    //}

    public class TextView : TemplatedControl
    {
        public static readonly PerspexProperty<bool> AcceptsReturnProperty =
            PerspexProperty.Register<TextView, bool>("AcceptsReturn");

        public static readonly PerspexProperty<bool> AcceptsTabProperty =
            PerspexProperty.Register<TextView, bool>("AcceptsTab");

        public static readonly PerspexProperty<int> CaretIndexProperty =
            PerspexProperty.Register<TextView, int>("CaretIndex", validate: ValidateCaretIndex);

        public static readonly PerspexProperty<int> SelectionStartProperty =
            PerspexProperty.Register<TextView, int>("SelectionStart", validate: ValidateCaretIndex);

        public static readonly PerspexProperty<int> SelectionEndProperty =
            PerspexProperty.Register<TextView, int>("SelectionEnd", validate: ValidateCaretIndex);

        public static readonly PerspexProperty<string> TextProperty =
            TextBlock.TextProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<TextWrapping> TextWrappingProperty =
            TextBlock.TextWrappingProperty.AddOwner<TextView>();

        public static readonly PerspexProperty<string> WatermarkProperty =
            PerspexProperty.Register<TextView, string>("Watermark");

        public static readonly PerspexProperty<bool> UseFloatingWatermarkProperty =
            PerspexProperty.Register<TextView, bool>("UseFloatingWatermark");

        private TextPresenter _presenter;

        static TextView()
        {
            FocusableProperty.OverrideDefaultValue(typeof(TextView), true);
        }

        public TextView()
        {
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
        }

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
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

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public bool UseFloatingWatermark
        {
            get { return GetValue(UseFloatingWatermarkProperty); }
            set { SetValue(UseFloatingWatermarkProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        protected override void OnTemplateApplied()
        {
            _presenter = this.GetTemplateChild<TextPresenter>("textPresenter");
            _presenter.Cursor = new Cursor(StandardCursorType.Ibeam);
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            _presenter.ShowCaret();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            SelectionStart = 0;
            SelectionEnd = 0;
            _presenter.HideCaret();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            HandleTextInput(e.Text);
        }

        private void HandleTextInput(string input)
        {
            string text = Text ?? string.Empty;
            int caretIndex = CaretIndex;
            if (!string.IsNullOrEmpty(input))
            {
                DeleteSelection();
                caretIndex = CaretIndex;
                text = Text ?? string.Empty;
                Text = text.Substring(0, caretIndex) + input + text.Substring(caretIndex);
                CaretIndex += input.Length;
                SelectionStart = SelectionEnd = CaretIndex;
            }
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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            string text = Text ?? string.Empty;
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
                        Text = text.Substring(0, caretIndex - 1) + text.Substring(caretIndex);
                        --CaretIndex;
                    }

                    break;

                case Key.Delete:
                    if (!DeleteSelection() && caretIndex < text.Length)
                    {
                        Text = text.Substring(0, caretIndex) + text.Substring(caretIndex + 1);
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

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            if (e.Source == _presenter)
            {
                var point = e.GetPosition(_presenter);
                var index = CaretIndex = _presenter.GetCaretIndex(point);
                var text = Text;

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
                        SelectionEnd = text.Length;
                        break;
                }

                e.Device.Capture(_presenter);
                e.Handled = true;
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (e.Device.Captured == _presenter)
            {
                var point = e.GetPosition(_presenter);
                CaretIndex = SelectionEnd = _presenter.GetCaretIndex(point);
            }
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (e.Device.Captured == _presenter)
            {
                e.Device.Capture(null);
            }
        }

        private static int ValidateCaretIndex(PerspexObject o, int value)
        {
            var text = o.GetValue(TextProperty);
            var length = (text != null) ? text.Length : 0;
            return Math.Max(0, Math.Min(length, value));
        }

        private void MoveHorizontal(int count, InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                if (count > 0)
                {
                   // count = StringUtils.NextWord(text, caretIndex, false) - caretIndex;
                }
                else
                {
                   // count = StringUtils.PreviousWord(text, caretIndex, false) - caretIndex;
                }
            }

            CaretIndex = caretIndex += count;
        }

        private void MoveVertical(int count, InputModifiers modifiers)
        {
            var formattedText = _presenter.FormattedText;
            var lines = formattedText.GetLines().ToList();
            var caretIndex = CaretIndex;
            var lineIndex = GetLine(caretIndex, lines) + count;

            if (lineIndex >= 0 && lineIndex < lines.Count)
            {
                var line = lines[lineIndex];
                var rect = formattedText.HitTestTextPosition(caretIndex);
                var y = count < 0 ? rect.Y : rect.Bottom;
                var point = new Point(rect.X, y + (count * (line.Height / 2)));
                var hit = formattedText.HitTestPoint(point);
                CaretIndex = hit.TextPosition + (hit.IsTrailing ? 1 : 0);
            }
        }

        private void MoveHome(InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = 0;
            }
            else
            {
                var lines = _presenter.FormattedText.GetLines();
                var pos = 0;

                foreach (var line in lines)
                {
                    if (pos + line.Length > caretIndex || pos + line.Length == text.Length)
                    {
                        break;
                    }

                    pos += line.Length;
                }

                caretIndex = pos;
            }

            CaretIndex = caretIndex;
        }

        private void MoveEnd(InputModifiers modifiers)
        {
            var text = Text ?? string.Empty;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = text.Length;
            }
            else
            {
                var lines = _presenter.FormattedText.GetLines();
                var pos = 0;

                foreach (var line in lines)
                {
                    pos += line.Length;

                    if (pos > caretIndex)
                    {
                        if (pos < text.Length)
                        {
                            --pos;
                        }

                        break;
                    }
                }

                caretIndex = pos;
            }

            CaretIndex = caretIndex;
        }

        private void SelectAll()
        {
            SelectionStart = 0;
            SelectionEnd = Text.Length;
        }

        private bool DeleteSelection()
        {
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;

            if (selectionStart != selectionEnd)
            {
                var start = Math.Min(selectionStart, selectionEnd);
                var end = Math.Max(selectionStart, selectionEnd);
                var text = Text;
                Text = text.Substring(0, start) + text.Substring(end);
                SelectionStart = SelectionEnd = CaretIndex = start;
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void Render(IDrawingContext context)
        {
            

            base.Render(context);
        }

        private string GetSelection()
        {
            var selectionStart = SelectionStart;
            var selectionEnd = SelectionEnd;
            var start = Math.Min(selectionStart, selectionEnd);
            var end = Math.Max(selectionStart, selectionEnd);
            if (start == end || (Text?.Length ?? 0) < end)
            {
                return "";
            }
            return Text.Substring(start, end - start);
        }

        private int GetLine(int caretIndex, IList<FormattedTextLine> lines)
        {
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
    }

    public class TextViewStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextViewStyle"/> class.
        /// </summary>
        public TextViewStyle()
        {
            AddRange(new[]
            {
                new Style(x => x.OfType<TextView>())
                {
                    Setters = new[]
                    {
                        new Setter(TemplatedControl.TemplateProperty, new ControlTemplate<TextView>(Template)),
                        new Setter(TemplatedControl.BackgroundProperty, Brush.Parse("#1e1e1e")),
                        new Setter(TemplatedControl.BorderBrushProperty, new SolidColorBrush(0xff707070)),
                        new Setter(TemplatedControl.BorderThicknessProperty, 2.0),
                        new Setter(Control.FocusAdornerProperty, null),
                    },
                },
                new Style(x => x.OfType<TextView>().Class(":focus").Template().Name("border"))
                {
                    Setters = new[]
                    {
                        new Setter(TemplatedControl.BorderBrushProperty, Brushes.Black),
                    },
                }
            });
        }

        /// <summary>
        /// The default template for the <see cref="TextView"/> control.
        /// </summary>
        /// <param name="control">The control being styled.</param>
        /// <returns>The root of the instantiated template.</returns>
        public static Control Template(TextView control)
        {
            Border result = new Border
            {
                Name = "border",
                Padding = new Thickness(2),
                [~Border.BackgroundProperty] = control[~TemplatedControl.BackgroundProperty],
                [~Border.BorderBrushProperty] = control[~TemplatedControl.BorderBrushProperty],
                [~Border.BorderThicknessProperty] = control[~TemplatedControl.BorderThicknessProperty],

                Child = new ScrollViewer
                {
                    [~ScrollViewer.CanScrollHorizontallyProperty] = control[~ScrollViewer.CanScrollHorizontallyProperty],
                    [~ScrollViewer.HorizontalScrollBarVisibilityProperty] = control[~ScrollViewer.HorizontalScrollBarVisibilityProperty],
                    [~ScrollViewer.VerticalScrollBarVisibilityProperty] = control[~ScrollViewer.VerticalScrollBarVisibilityProperty],
                    Content = new StackPanel
                    {
                        Children = new Perspex.Controls.Controls
                        {
                            new TextBlock
                            {
                                Name  = "floatingWatermark",
                                Foreground = SolidColorBrush.Parse("#007ACC"),
                                FontSize = 10,
                                [~TextBlock.TextProperty] = control[~TextView.WatermarkProperty],
                                [~TextBlock.IsVisibleProperty] = control[~TextView.TextProperty].Cast<string>().Select(x => (object)(!string.IsNullOrEmpty(x) && control.UseFloatingWatermark))
                            },
                            new Panel
                            {
                                Children = new Perspex.Controls.Controls
                                {
                                    new TextBlock
                                    {
                                        Name = "watermark",
                                        Opacity = 0.5,
                                        [~TextBlock.TextProperty] = control[~TextView.WatermarkProperty],
                                        [~TextBlock.IsVisibleProperty] = control[~TextView.TextProperty].Cast<string>().Select(x => (object)string.IsNullOrEmpty(x))
                                    },
                                    new TextPresenter
                                    {
                                        Name = "textPresenter",
                                        [~TextPresenter.CaretIndexProperty] = control[~TextView.CaretIndexProperty],
                                        [~TextPresenter.SelectionStartProperty] = control[~TextView.SelectionStartProperty],
                                        [~TextPresenter.SelectionEndProperty] = control[~TextView.SelectionEndProperty],
                                        [~TextBlock.TextProperty] = control[~TextView.TextProperty],
                                        [~TextBlock.TextWrappingProperty] = control[~TextView.TextWrappingProperty],
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return result;
        }
    }
}
