﻿namespace AvalonStudio.TextEditor
{
    using Utils;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Input;
    using Perspex.Input.Platform;
    using Perspex.Interactivity;
    using Perspex.Media;
    using Perspex.Threading;
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reactive.Linq;
    using Document;
    using Rendering;
    using Models.LanguageServices;
    using System.Collections.Generic;

    public class TextEditor : TemplatedControl
    {
        #region Contructors
        static TextEditor()
        {
            TextChangedDelayProperty.Changed.AddClassHandler<TextEditor>((s, v) => s.textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)v.NewValue));

            FocusableProperty.OverrideDefaultValue(typeof(TextEditor), true);

            SyntaxHighlightingDataProperty.Changed.AddClassHandler<TextEditor>((s, v) => s.InvalidateVisual());
        }

        public TextEditor()
        {
            Name = "textEditor";

            textChangedDelayTimer = new DispatcherTimer();
            textChangedDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            textChangedDelayTimer.Tick += TextChangedDelayTimer_Tick;
            textChangedDelayTimer.Stop();

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

            SyntaxHighlightingDataProperty.Changed.Subscribe((args) =>
            {
                if (TextColorizer != null)
                {
                    TextColorizer.SetTransformations();
                }
            });

            DiagnosticsProperty.Changed.Subscribe((args) =>
            {
                if(textMarkerService != null && args.NewValue != null)
                {
                    var diags = args.NewValue as List<Diagnostic>;

                    textMarkerService.Clear();

                    foreach(var diag in diags)
                    {
                        var endoffset = TextUtilities.GetNextCaretPosition(TextDocument, diag.Offset, TextUtilities.LogicalDirection.Forward, TextUtilities.CaretPositioningMode.WordBorderOrSymbol);
                        var length = endoffset - diag.Offset;
                        textMarkerService.Create(diag.Offset, length, diag.Spelling, Color.FromRgb(243, 45, 45));
                    }
                }

            });
        }
        #endregion

        #region Private Data
        private TextView textView;
        private StackPanel marginsContainer;
        private readonly DispatcherTimer textChangedDelayTimer;
        #endregion

        #region Pespex Properties
        public static readonly PerspexProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
        TextView.BeforeTextChangedCommandProperty.AddOwner<TextEditor>();

        public System.Windows.Input.ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
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
            set { SetValue(CaretIndexProperty, value); }
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

        public static readonly PerspexProperty<ObservableCollection<TextEditorMargin>> MarginsProperty =
            PerspexProperty.Register<TextEditor, ObservableCollection<TextEditorMargin>>(nameof(Margins));

        public ObservableCollection<TextEditorMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<SyntaxHighlightingData>> SyntaxHighlightingDataProperty =
            PerspexProperty.Register<TextEditor, ObservableCollection<SyntaxHighlightingData>>(nameof(SyntaxHighlightingData));

        public ObservableCollection<SyntaxHighlightingData> SyntaxHighlightingData
        {
            get { return GetValue(SyntaxHighlightingDataProperty); }
            set { SetValue(SyntaxHighlightingDataProperty, value); }
        }

        public static readonly PerspexProperty<List<Diagnostic>> DiagnosticsProperty =
            PerspexProperty.Register<TextEditor, List<Diagnostic>>(nameof(Diagnostics));

        public List<Diagnostic> Diagnostics
        {
            get { return GetValue(DiagnosticsProperty); }
            set { SetValue(DiagnosticsProperty, value); }
        }

        public static readonly PerspexProperty<Brush> PunctuationBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(PunctuationBrush));

        public Brush PunctuationBrush
        {
            get { return GetValue(PunctuationBrushProperty); }
            set { SetValue(PunctuationBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> KeywordBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(KeywordBrush));

        public Brush KeywordBrush
        {
            get { return GetValue(KeywordBrushProperty); }
            set { SetValue(KeywordBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> IdentifierBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(IdentifierBrush));

        public Brush IdentifierBrush
        {
            get { return GetValue(IdentifierBrushProperty); }
            set { SetValue(IdentifierBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> LiteralBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(LiteralBrush));

        public Brush LiteralBrush
        {
            get { return GetValue(LiteralBrushProperty); }
            set { SetValue(LiteralBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> UserTypeBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(UserTypeBrush));

        public Brush UserTypeBrush
        {
            get { return GetValue(UserTypeBrushProperty); }
            set { SetValue(UserTypeBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> CommentBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>(nameof(CommentBrush));

        public Brush CommentBrush
        {
            get { return GetValue(CommentBrushProperty); }
            set { SetValue(CommentBrushProperty, value); }
        }

        public static readonly PerspexProperty<TextDocument> TextDocumentProperty = TextView.TextDocumentProperty.AddOwner<TextEditor>();

        public TextDocument TextDocument
        {
            get { return GetValue(TextDocumentProperty); }
            set { SetValue(TextDocumentProperty, value); }
        }
        #endregion

        #region Properties
        public TextColoringTransformer TextColorizer { get; set; }
        public TextView TextView { get { return textView; } }
        public ScrollViewer ScrollViewer { get; set; }
        #endregion

        #region Private Methods
        private void HandleTextInput(string input)
        {
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
            var lineIndex = textView.GetLine(caretIndex) + count;

            if (lineIndex > 0 && lineIndex <= TextDocument.LineCount)
            {
                var line = TextDocument.Lines[lineIndex - 1];

                var rect = VisualLineGeometryBuilder.GetTextPosition(textView, caretIndex);
                var y = count < 0 ? rect.Y : rect.Bottom;
                var point = new Point(rect.X, y + (count * (textView.CharSize.Height / 2)));

                CaretIndex = textView.GetOffsetFromPoint(point);
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
        public void InstallMargin(Control margin)
        {
            marginsContainer.Children.Add(margin);
        }
        #endregion

        private TextMarkerService textMarkerService;

        #region Overrides
        protected override void OnTemplateApplied(INameScope nameScope)
        {
            textView = nameScope.Find<TextView>("textView");
            textView.Cursor = new Cursor(StandardCursorType.Ibeam);            

            marginsContainer = nameScope.Find<StackPanel>("marginContainer");

            InstallMargin(new BreakPointMargin());
            InstallMargin(new LineNumberMargin());

            TextDocumentProperty.Changed.Subscribe((args) =>
            {
                if (args.NewValue != null)
                {
                    textView.BackgroundRenderers.Clear();
                    textView.DocumentLineTransformers.Clear();

                    TextColorizer = new TextColoringTransformer(this);
                    textView.BackgroundRenderers.Add(new SelectedLineBackgroundRenderer());
                    textView.BackgroundRenderers.Add(new SelectionBackgroundRenderer());
                    textView.DocumentLineTransformers.Add(TextColorizer);
                    textMarkerService = new TextMarkerService(this);
                    textView.BackgroundRenderers.Add(textMarkerService);

                    TextDocument.Changing += (sender, e) =>
                    {
                        if (BeforeTextChangedCommand != null)
                        {
                            BeforeTextChangedCommand.Execute(null);
                        }
                    };

                    TextDocument.Changed += (sender, e) =>
                    {
                        textChangedDelayTimer.Stop();
                        textChangedDelayTimer.Start();
                    };
                }
            });


            ScrollViewer = nameScope.Find<ScrollViewer>("scrollViewer");
        }

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            if (e.Source == textView)
            {
                var point = e.GetPosition(textView);
                var index = CaretIndex = textView.GetOffsetFromPoint(point);
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

                e.Device.Capture(textView);
                e.Handled = true;
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (e.Device.Captured == textView)
            {
                var point = e.GetPosition(textView);
                CaretIndex = SelectionEnd = textView.GetOffsetFromPoint(point);
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
        #endregion       
    }
}
