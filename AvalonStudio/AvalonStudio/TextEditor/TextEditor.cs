namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Input;
    using Perspex.Input.Platform;
    using Perspex.Interactivity;
    using Perspex.Media;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;

    public class TextEditor : TemplatedControl
    {
        #region Contructors
        static TextEditor()
        {
            FocusableProperty.OverrideDefaultValue(typeof(TextEditor), true);
        }

        public TextEditor()
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
        #endregion

        #region Private Data
        private TextView textView;
        private StackPanel marginsContainer;
        #endregion

        #region Pespex Properties
        public static readonly PerspexProperty<string> TextProperty =
            PerspexProperty.Register<TextEditor, string>("Text");

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly PerspexProperty<TextWrapping> TextWrappingProperty =
        TextBlock.TextWrappingProperty.AddOwner<TextEditor>();

        public TextWrapping TextWrapping
        {
            get { return GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsReturnProperty =
            PerspexProperty.Register<TextEditor, bool>("AcceptsReturn", defaultValue: true);

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsTabProperty =
            PerspexProperty.Register<TextEditor, bool>("AcceptsTab");

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly PerspexProperty<int> CaretIndexProperty =
            PerspexProperty.Register<TextEditor, int>("CaretIndex", validate: ValidateCaretIndex, defaultBindingMode: BindingMode.TwoWay);

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionStartProperty =
            PerspexProperty.Register<TextEditor, int>("SelectionStart", validate: ValidateCaretIndex);

        public int SelectionStart
        {
            get { return GetValue(SelectionStartProperty); }
            set { SetValue(SelectionStartProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionEndProperty =
            PerspexProperty.Register<TextEditor, int>("SelectionEnd", validate: ValidateCaretIndex);

        public int SelectionEnd
        {
            get { return GetValue(SelectionEndProperty); }
            set { SetValue(SelectionEndProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<TextEditorMargin>> MarginsProperty = 
            PerspexProperty.Register<TextEditor, ObservableCollection<TextEditorMargin>>("Margins");

        public ObservableCollection<TextEditorMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }
        #endregion

        #region Private Methods
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

        //private int GetLine(int caretIndex, IList<FormattedTextLine> lines)
        //{
        //    int pos = 0;
        //    int i;

        //    for (i = 0; i < lines.Count; ++i)
        //    {
        //        var line = lines[i];
        //        pos += line.Length;

        //        if (pos > caretIndex)
        //        {
        //            break;
        //        }
        //    }

        //    return i;
        //}

        private static int ValidateCaretIndex(PerspexObject o, int value)
        {
            var text = o.GetValue(TextProperty);
            var length = (text != null) ? text.Length : 0;
            return Math.Max(0, Math.Min(length, value));
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
                    //count = StringUtils.PreviousWord(text, caretIndex, false) - caretIndex;
                }
            }

            CaretIndex = caretIndex += count;
        }

        private void MoveVertical(int count, InputModifiers modifiers)
        {
            var formattedText = textView.FormattedText;
            var lines = formattedText.GetLines().ToList();
            var caretIndex = CaretIndex;
            var lineIndex = textView.GetLine(caretIndex) + count;

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
                var lines = textView.FormattedText.GetLines();
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
                var lines = textView.FormattedText.GetLines();
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

        public void InstallMargin (Control margin)
        {
            marginsContainer.Children.Add(margin);
        }

        #region Overrides
        protected override void OnTemplateApplied()
        {
            textView = this.GetTemplateChild<TextView>("textView");            
            textView.Cursor = new Cursor(StandardCursorType.Ibeam);

            marginsContainer = this.GetTemplateChild<StackPanel>("marginContainer");

            InstallMargin(new BreakPointMargin(textView));
            InstallMargin(new LineNumberMargin(textView));            
        }

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            if (e.Source == textView)
            {
                var point = e.GetPosition(textView);
                var index = CaretIndex = textView.GetCaretIndex(point);
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

                e.Device.Capture(textView);
                e.Handled = true;
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (e.Device.Captured == textView)
            {
                var point = e.GetPosition(textView);
                CaretIndex = SelectionEnd = textView.GetCaretIndex(point);
            }
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            if (e.Device.Captured == textView)
            {
                e.Device.Capture(null);
            }
        }

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            textView.ShowCaret();
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            SelectionStart = 0;
            SelectionEnd = 0;
            textView.HideCaret();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            HandleTextInput(e.Text);
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

        #endregion
    }
}
