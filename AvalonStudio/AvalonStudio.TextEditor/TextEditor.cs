namespace AvalonStudio.TextEditor
{
    using Document;
    using Indentation;
    using OmniXaml.Attributes;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Data;
    using Perspex.Input;
    using Perspex.Input.Platform;
    using Perspex.Interactivity;
    using Perspex.Media;
    using Perspex.Threading;
    using Rendering;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;

    [ContentProperty("Content")]
    public class TextEditor : TemplatedControl
    {
        #region Contructors
        static TextEditor()
        {
            TextChangedDelayProperty.Changed.AddClassHandler<TextEditor>((s, v) => s.textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)v.NewValue));

            FocusableProperty.OverrideDefaultValue(typeof(TextEditor), true);
        }

        public TextEditor()
        {
            Styles.Add(new TextEditorTheme());

            Name = "textEditor";
            highestUserSelectedColumn = 1;
            textChangedDelayTimer = new DispatcherTimer();
            textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, 225);
            textChangedDelayTimer.Tick += TextChangedDelayTimer_Tick;
            textChangedDelayTimer.Stop();

            var canScrollHorizontally = this.GetObservable(AcceptsReturnProperty)
               .Select(x => !x);

            //Bind(
            //    ScrollViewer.CanScrollHorizontallyProperty,
            //    canScrollHorizontally,
            //    BindingPriority.Style);

            var horizontalScrollBarVisibility = this.GetObservable(AcceptsReturnProperty)
                .Select(x => x ? ScrollBarVisibility.Auto : ScrollBarVisibility.Hidden);

            Bind(
                ScrollViewer.HorizontalScrollBarVisibilityProperty,
                horizontalScrollBarVisibility,
                BindingPriority.Style);

            TextDocumentProperty.Changed.Subscribe((e) =>
            {
                CaretIndex = -1;
            });

            AddHandler(InputElement.KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        }
        #endregion

        public void ScrollToLine(int line)
        {
            textView.ScrollToLine(line);
        }

        #region Private Data
        private TextView textView;
        private readonly DispatcherTimer textChangedDelayTimer;
        private readonly DispatcherTimer mouseHoverDelayTimer;
        private int highestUserSelectedColumn;
        #endregion

        #region Pespex Properties
        public static readonly PerspexProperty<string> TabCharacterProperty =
            PerspexProperty.Register<TextEditor, string>(nameof(TabCharacter), defaultValue: "    ");

        public string TabCharacter
        {
            get { return GetValue(TabCharacterProperty); }
            set { SetValue(TabCharacterProperty, value); }
        }

        public static readonly PerspexProperty<int> MouseCursorOffsetProperty =
            PerspexProperty.Register<TextEditor, int>(nameof(MouseCursorOffset));

        public int MouseCursorOffset
        {
            get { return GetValue(MouseCursorOffsetProperty); }
            set { SetValue(MouseCursorOffsetProperty, value); }
        }

        public static readonly PerspexProperty<Point> MouseCursorPositionProperty =
            PerspexProperty.Register<TextEditor, Point>(nameof(MouseCursorPosition), defaultBindingMode: BindingMode.TwoWay);

        public Point MouseCursorPosition
        {
            get { return GetValue(MouseCursorPositionProperty); }
            set { SetValue(MouseCursorPositionProperty, value); }
        }

        public static readonly PerspexProperty<string> SelectedWordProperty =
            PerspexProperty.Register<TextEditor, string>(nameof(SelectedWord), defaultValue: string.Empty, defaultBindingMode: BindingMode.TwoWay);

        public string SelectedWord
        {
            get { return GetValue(SelectedWordProperty); }
            set { SetValue(SelectedWordProperty, value); }
        }

        public static readonly PerspexProperty<double> LineHeightProperty =
            PerspexProperty.Register<TextEditor, double>(nameof(LineHeight), defaultBindingMode: BindingMode.TwoWay);

        public double LineHeight
        {
            get { return GetValue(LineHeightProperty); }
            set { SetValue(LineHeightProperty, value); }
        }

        public static readonly StyledProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
        TextView.BeforeTextChangedCommandProperty.AddOwner<TextEditor>();

        public System.Windows.Input.ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly StyledProperty<object> ContentProperty = TextView.ContentProperty.AddOwner<TextEditor>();

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Defines the <see cref="Header"/> property.
        /// </summary>
        public static readonly StyledProperty<object> HeaderProperty =
            PerspexProperty.Register<TextEditor, object>(nameof(Header));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public static readonly StyledProperty<ObservableCollection<TextViewMargin>> MarginsProperty =
            TextView.MarginsProperty.AddOwner<TextEditor>();

        public ObservableCollection<TextViewMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<IBackgroundRenderer>> BackgroundRenderersProperty =
            TextView.BackgroundRenderersProperty.AddOwner<TextEditor>();

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return GetValue(BackgroundRenderersProperty); }
            set { SetValue(BackgroundRenderersProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<IDocumentLineTransformer>> DocumentLineTransformersProperty =
            TextView.DocumentLineTransformersProperty.AddOwner<TextEditor>();

        public ObservableCollection<IDocumentLineTransformer> DocumentLineTransformers
        {
            get { return GetValue(DocumentLineTransformersProperty); }
            set { SetValue(DocumentLineTransformersProperty, value); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> TextChangedCommandProperty =
            TextView.TextChangedCommandProperty.AddOwner<TextEditor>();

        public System.Windows.Input.ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<int> TextChangedDelayProperty =
                    PerspexProperty.Register<TextEditor, int>(nameof(TextChangedDelay));

        public int TextChangedDelay
        {
            get { return GetValue(TextChangedDelayProperty); }
            set { SetValue(TextChangedDelayProperty, value); textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsReturnProperty =
            PerspexProperty.Register<TextEditor, bool>(nameof(AcceptsReturn));

        public bool AcceptsReturn
        {
            get { return GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly PerspexProperty<bool> AcceptsTabProperty =
            PerspexProperty.Register<TextEditor, bool>(nameof(AcceptsTab));

        public bool AcceptsTab
        {
            get { return GetValue(AcceptsTabProperty); }
            set { SetValue(AcceptsTabProperty, value); }
        }

        public static readonly PerspexProperty<int> CaretIndexProperty =
            TextView.CaretIndexProperty.AddOwner<TextEditor>();

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set
            {
                SetValue(CaretIndexProperty, value);

                if (TextDocument != null && CaretIndex != -1)
                {
                    InvalidateCaretPosition();

                    InvalidateSelectedWord();
                }
            }
        }

        public static readonly PerspexProperty<Point> CaretLocationProperty =
            PerspexProperty.Register<TextEditor, Point>(nameof(CaretLocation), defaultBindingMode: BindingMode.TwoWay);

        public Point CaretLocation
        {
            get { return GetValue(CaretLocationProperty); }
            set { SetValue(CaretLocationProperty, value); }
        }

        public static readonly PerspexProperty<Point> CaretLocationInTextViewProperty =
            PerspexProperty.Register<TextEditor, Point>(nameof(CaretLocationInTextView), defaultBindingMode: BindingMode.TwoWay);

        public Point CaretLocationInTextView
        {
            get { return GetValue(CaretLocationInTextViewProperty); }
            set { SetValue(CaretLocationInTextViewProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionStartProperty =
            PerspexProperty.Register<TextEditor, int>(nameof(SelectionStart));

        public int SelectionStart
        {
            get { return GetValue(SelectionStartProperty); }
            set { SetValue(SelectionStartProperty, value); }
        }

        public static readonly PerspexProperty<int> SelectionEndProperty =
            PerspexProperty.Register<TextEditor, int>(nameof(SelectionEnd));

        public int SelectionEnd
        {
            get { return GetValue(SelectionEndProperty); }
            set { SetValue(SelectionEndProperty, value); }
        }

        public TextSegment GetSelectionAsSegment()
        {
            TextSegment result = null;

            if (SelectionStart < SelectionEnd)
            {
                result = new TextSegment() { StartOffset = SelectionStart, EndOffset = SelectionEnd };
            }
            else
            {
                result = new TextSegment() { StartOffset = SelectionEnd, EndOffset = SelectionStart };
            }

            return result;
        }

        public void SetSelection(TextSegment segment)
        {
            SelectionStart = segment.StartOffset;
            SelectionEnd = segment.EndOffset;
        }

        public static readonly PerspexProperty<IIndentationStrategy> IndentationStrategyProperty = PerspexProperty.Register<TextEditor, IIndentationStrategy>(nameof(IndentationStrategy));

        public IIndentationStrategy IndentationStrategy
        {
            get { return GetValue(IndentationStrategyProperty); }
            set { SetValue(IndentationStrategyProperty, value); }
        }

        public static readonly StyledProperty<TextDocument> TextDocumentProperty = TextView.TextDocumentProperty.AddOwner<TextEditor>();

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); }
        }
        #endregion

        #region Properties
        public TextView TextView { get { return textView; } }
        public ScrollViewer ScrollViewer { get; set; }
        #endregion

        #region Private Methods
        private void InvalidateCaretPosition()
        {
            CaretLocation = VisualLineGeometryBuilder.GetTextViewPosition(TextView, CaretIndex).TopLeft;
            CaretLocationInTextView = new Point(CaretLocation.X - TextView.TextSurfaceBounds.X - TextView.CharSize.Width, CaretLocation.Y + TextView.CharSize.Height);
        }

        private void InvalidateSelectedWord()
        {
            if (CaretIndex >= 0 && TextDocument.TextLength > CaretIndex)
            {
                bool wordFound = false;

                int start = CaretIndex;

                var currentChar = TextDocument.GetCharAt(CaretIndex);
                char prevChar = '\0';

                if (CaretIndex > 0)
                {
                    prevChar = TextDocument.GetCharAt(CaretIndex - 1);
                }

                var charClass = TextUtilities.GetCharacterClass(currentChar);

                if (charClass != TextUtilities.CharacterClass.LineTerminator && prevChar != ' ' && TextUtilities.GetCharacterClass(prevChar) != TextUtilities.CharacterClass.LineTerminator)
                {
                    start = TextUtilities.GetNextCaretPosition(TextDocument, CaretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);
                }

                int end = TextUtilities.GetNextCaretPosition(TextDocument, start, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorder);

                if (start != -1 && end != -1)
                {
                    string word = TextDocument.GetText(start, end - start).Trim();

                    if (TextUtilities.IsSymbol(word))
                    {
                        SelectedWord = word;
                        wordFound = true;
                    }

                    if (!wordFound)
                    {
                        SelectedWord = string.Empty;
                    }
                }
            }
        }

        private void HandleTextInput(string input)
        {
            InvalidateSelectedWord();
            //string text = TextDocument ?? string.Empty;
            int caretIndex = CaretIndex;

            if (!string.IsNullOrEmpty(input))
            {
                DeleteSelection();
                caretIndex = CaretIndex;
                TextDocument.Insert(caretIndex, input);
                CaretIndex += input.Length;
                SelectionStart = SelectionEnd = CaretIndex;
                TextView.Invalidate();
            }
        }

        private void TextChangedDelayTimer_Tick(object sender, EventArgs e)
        {
            textChangedDelayTimer.Stop();

            if (TextChangedCommand != null)
            {
                TextChangedCommand.Execute(null);
            }
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
                TextDocument.Remove(start, end - start);
                TextView.Invalidate();

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

        private void SetHighestColumn()
        {
            if (CaretIndex != -1)
            {
                highestUserSelectedColumn = TextDocument.GetLocation(CaretIndex).Column;
            }
        }


        private void MoveHorizontal(int count, InputModifiers modifiers)
        {
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                if (count > 0)
                {
                    count = TextUtilities.GetNextCaretPosition(TextDocument, caretIndex, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordStartOrSymbol) - caretIndex;
                }
                else
                {
                    count = TextUtilities.GetNextCaretPosition(TextDocument, caretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStartOrSymbol) - caretIndex;
                }

                if (caretIndex + count <= TextDocument.TextLength && caretIndex + count >= 0)
                {
                    CaretIndex += count;
                }
            }
            else
            {
                if (count > 0)
                {
                    for (int i = 0; i < Math.Abs(count); i++)
                    {
                        var line = TextDocument.GetLineByOffset(CaretIndex);

                        if (caretIndex == line.EndOffset && line.NextLine != null)
                        {
                            caretIndex = line.NextLine.Offset;
                        }
                        else
                        {
                            caretIndex = TextUtilities.GetNextCaretPosition(TextDocument, caretIndex, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.Normal);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Math.Abs(count); i++)
                    {
                        var line = TextDocument.GetLineByOffset(CaretIndex);

                        if (caretIndex == line.Offset && line.PreviousLine != null)
                        {
                            caretIndex = line.PreviousLine.EndOffset;
                        }
                        else
                        {
                            caretIndex = TextUtilities.GetNextCaretPosition(TextDocument, caretIndex, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.Normal);
                        }
                    }
                }

                CaretIndex = caretIndex;
            }
            
            SetHighestColumn();
        }

        private void MoveVertical(int count, InputModifiers modifiers)
        {
            var caretIndex = CaretIndex;
            var currentPosition = TextDocument.GetLocation(caretIndex);

            if (currentPosition.Line + count > 0 && currentPosition.Line + count <= TextDocument.LineCount)
            {
                var line = TextDocument.Lines[currentPosition.Line - 1 + count];

                var col = line.EndOffset;

                if (highestUserSelectedColumn <= line.Length)
                {
                    col = highestUserSelectedColumn;
                }

                CaretIndex = TextDocument.GetOffset(currentPosition.Line + count, col);
            }
        }

        private void MoveHome(InputModifiers modifiers)
        {
            var text = TextDocument ?? null;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = 0;
            }
            else
            {
                var lineOffset = TextDocument.GetLineByOffset(CaretIndex).Offset;
                var whiteSpace = TextUtilities.GetWhitespaceAfter(TextDocument, lineOffset);
                caretIndex = lineOffset + whiteSpace.Length;
            }

            CaretIndex = caretIndex;
            SetHighestColumn();
        }

        private void MoveEnd(InputModifiers modifiers)
        {
            var text = TextDocument ?? null;
            var caretIndex = CaretIndex;

            if ((modifiers & InputModifiers.Control) != 0)
            {
                caretIndex = TextDocument.TextLength;
            }
            else
            {
                var lineOffset = TextDocument.GetLineByOffset(CaretIndex).EndOffset;
                var whiteSpace = TextUtilities.GetWhitespaceBefore(TextDocument, lineOffset);
                caretIndex = lineOffset - whiteSpace.Length;
            }

            CaretIndex = caretIndex;
            SetHighestColumn();
        }

        private async void Cut()
        {
            await ((IClipboard)PerspexLocator.Current.GetService(typeof(IClipboard)))
                .SetTextAsync(GetSelection());

            DeleteSelection();
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

        private void Undo()
        {
            TextDocument?.UndoStack.Undo();
        }

        private void Redo()
        {
            TextDocument?.UndoStack.Redo();
        }

        sealed class RestoreCaretAndSelectionUndoAction : IUndoableOperation
        {
            // keep textarea in weak reference because the IUndoableOperation is stored with the document
            WeakReference textAreaReference;
            int caretPosition;
            int selectionStart;
            int selectionEnd;

            public RestoreCaretAndSelectionUndoAction(TextEditor editor)
            {
                this.textAreaReference = new WeakReference(editor);
                // Just save the old caret position, no need to validate here.
                // If we restore it, we'll validate it anyways.
                this.caretPosition = editor.CaretIndex;
                this.selectionStart = editor.SelectionStart;
                this.selectionEnd = editor.SelectionEnd;
            }

            public void Undo()
            {
                var textEditor = (TextEditor)textAreaReference.Target;
                if (textEditor != null)
                {
                    textEditor.CaretIndex = caretPosition;
                    textEditor.SelectionStart = selectionStart;
                    textEditor.SelectionEnd = selectionEnd;
                }
            }

            public void Redo()
            {
                // redo=undo: we just restore the caret/selection state
                Undo();
            }
        }

        #endregion

        #region Public Methods        
        #endregion

        #region Overrides
        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            textView = e.NameScope.Find<TextView>("textView");
            textView.Cursor = new Cursor(StandardCursorType.Ibeam);

            //textView.BackgroundRenderers.Clear();
            //textView.DocumentLineTransformers.Clear();

            //textView.BackgroundRenderers.Add(new SelectedLineBackgroundRenderer());
            //textView.BackgroundRenderers.Add(new ColumnLimitBackgroundRenderer());
            //textView.BackgroundRenderers.Add(new SelectionBackgroundRenderer());
            //textView.DocumentLineTransformers.Add(new SelectedWordTextLineTransformer(this));

            TextDocumentProperty.Changed.Subscribe((args) =>
            {
                if (args.NewValue != null)
                {
                    TextDocument.Changing += (sender, ee) =>
                    {
                        TextDocument?.UndoStack.StartUndoGroup();
                        TextDocument?.UndoStack.PushOptional(new RestoreCaretAndSelectionUndoAction(this));

                        if (BeforeTextChangedCommand != null)
                        {
                            BeforeTextChangedCommand.Execute(null);
                        }
                    };

                    TextDocument.Changed += (sender, ee) =>
                    {
                        textChangedDelayTimer.Stop();
                        textChangedDelayTimer.Start();

                        TextDocument?.UndoStack.EndUndoGroup();

                        InvalidateVisual();

                        LineHeight = textView.CharSize.Height;
                    };
                }
            });

            ScrollViewer = e.NameScope.Find<ScrollViewer>("scrollViewer");
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e.Source.InteractiveParent.InteractiveParent == textView)
            {
                var point = e.GetPosition(textView.TextSurface);

                var index = textView.GetOffsetFromPoint(point);

                if (index != -1)
                {
                    CaretIndex = index;

                    var text = TextDocument;

                    switch (e.ClickCount)
                    {
                        case 1:
                            SelectionStart = SelectionEnd = index;
                            break;
                        case 2:
                            SelectionStart = TextUtilities.GetNextCaretPosition(TextDocument, index, TextUtilities.LogicalDirection.Backward, TextUtilities.CaretPositioningMode.WordStart);

                            SelectionEnd = TextUtilities.GetNextCaretPosition(TextDocument, index, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorder);
                            break;
                        case 3:
                            SelectionStart = 0;
                            SelectionEnd = text.TextLength;
                            break;
                    }

                    e.Device.Capture(textView);
                    e.Handled = true;

                    InvalidateVisual();
                }
                else if (TextDocument.TextLength == 0)
                {
                    SelectionStart = SelectionEnd = CaretIndex = 0;

                    e.Device.Capture(textView);
                    e.Handled = true;

                    InvalidateVisual();
                }

                SetHighestColumn();
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var point = e.GetPosition(textView.TextSurface);

            var currentMouseOffset = textView.GetOffsetFromPoint(point);

            if (currentMouseOffset != -1)
            {
                if (e.Device.Captured == textView)
                {
                    CaretIndex = currentMouseOffset;

                    if (CaretIndex >= 0)
                    {
                        SelectionEnd = CaretIndex;
                    }
                    else
                    {
                        SelectionEnd = 0;
                    }
                }
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

            textView.HideCaret();
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            HandleTextInput(e.Text);
        }

        protected void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

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

                case Key.X:
                    if (modifiers == InputModifiers.Control)
                    {
                        Cut();
                    }
                    break;

                case Key.Y:
                    if (modifiers == InputModifiers.Control)
                    {
                        Redo();
                    }
                    break;

                case Key.Z:
                    if (modifiers == InputModifiers.Control)
                    {
                        Undo();
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
                        var line = TextDocument.GetLineByOffset(CaretIndex);

                        if (CaretIndex == line.Offset && line.PreviousLine != null)
                        {
                            TextDocument.Remove(CaretIndex - line.DelimiterLength, line.DelimiterLength);

                            CaretIndex -= line.DelimiterLength;
                        }
                        else
                        {
                            TextDocument.Remove(caretIndex - 1, 1);
                            --CaretIndex;
                        }

                        TextView.Invalidate();
                    }

                    break;

                case Key.Delete:
                    if (!DeleteSelection() && caretIndex < TextDocument.TextLength)
                    {
                        var line = TextDocument.GetLineByOffset(CaretIndex);

                        if (CaretIndex == line.EndOffset && line.NextLine != null)
                        {
                            TextDocument.Remove(CaretIndex, line.DelimiterLength);                            
                        }
                        else
                        {
                            TextDocument.Remove(caretIndex, 1);                            
                        }
                        
                        TextView.Invalidate();
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
                        e.Handled = true;

                        bool shiftedLines = false;

                        if (SelectionStart != SelectionEnd)
                        {
                            var selection = GetSelectionAsSegment();
                            var lines = VisualLineGeometryBuilder.GetLinesForSegmentInDocument(TextDocument, selection);

                            if (lines.Count() > 1)
                            {
                                var anchors = new TextSegmentCollection<TextSegment>(TextDocument);
                                
                                anchors.Add(selection);

                                TextDocument.BeginUpdate();

                                foreach (var line in lines)
                                {                                    
                                    TextDocument.Insert(line.Offset, TabCharacter);
                                }

                                TextDocument.EndUpdate();

                                SetSelection(selection);

                                shiftedLines = true;
                            }
                        }

                        if (!shiftedLines)
                        {
                            if (e.Modifiers == InputModifiers.Shift)
                            {
                                // TODO delete upto 4 whitespace chars.
                            }
                            else
                            {
                                HandleTextInput(TabCharacter);
                            }
                        }
                    }
                    else
                    {
                        base.OnKeyDown(e);
                        handled = false;
                    }

                    break;

                case Key.PageUp:
                    textView.PageUp();
                    break;

                case Key.PageDown:
                    textView.PageDown();
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
                InvalidateVisual();
            }
        }
        #endregion       
    }
}
