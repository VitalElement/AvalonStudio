namespace AvalonStudio.TextEditor
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
    using System.Windows.Input;
    public class TextEditor : TemplatedControl
    {
        #region Contructors
        static TextEditor()
        {
            FocusableProperty.OverrideDefaultValue(typeof(TextEditor), true);            

            SyntaxHighlightingDataProperty.Changed.AddClassHandler<TextEditor>((s, v) => s.InvalidateVisual());
        }

        public TextEditor()
        {
            Name = "textEditor";

            SyntaxHighlightingDataProperty.Changed.Subscribe((args) =>
            {
                TextColorizer.SetTransformations();
            });

            TextColorizer = new TextColoringTransformer(this);
        }        
        #endregion

        #region Private Data
        private TextView textView;
        private StackPanel marginsContainer;
        #endregion

        #region Pespex Properties
        public static readonly PerspexProperty<System.Windows.Input.ICommand> BeforeTextChangedCommandProperty =
        TextView.BeforeTextChangedCommandProperty.AddOwner<TextEditor>();

        public ICommand BeforeTextChangedCommand
        {
            get { return GetValue(BeforeTextChangedCommandProperty); }
            set { SetValue(BeforeTextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<System.Windows.Input.ICommand> TextChangedCommandProperty =
            TextView.TextChangedCommandProperty.AddOwner<TextEditor>();

        public ICommand TextChangedCommand
        {
            get { return GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public static readonly PerspexProperty<int> TextChangedDelayProperty =
            TextView.TextChangedDelayProperty.AddOwner<TextEditor>();

        public int TextChangedDelay
        {
            get { return GetValue(TextChangedDelayProperty); }
            set { SetValue(TextChangedDelayProperty, value); }
        }

        public static readonly PerspexProperty<int> CaretIndexProperty =
            TextView.CaretIndexProperty.AddOwner<TextEditor>();

        public int CaretIndex
        {
            get { return GetValue(CaretIndexProperty); }
            set { SetValue(CaretIndexProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<TextEditorMargin>> MarginsProperty =
            PerspexProperty.Register<TextEditor, ObservableCollection<TextEditorMargin>>("Margins");

        public ObservableCollection<TextEditorMargin> Margins
        {
            get { return GetValue(MarginsProperty); }
            set { SetValue(MarginsProperty, value); }
        }

        public static readonly PerspexProperty<ObservableCollection<SyntaxHighlightingData>> SyntaxHighlightingDataProperty =
            PerspexProperty.Register<TextEditor, ObservableCollection<SyntaxHighlightingData>>("SyntaxHighlightingData");

        public ObservableCollection<SyntaxHighlightingData> SyntaxHighlightingData
        {
            get { return GetValue(SyntaxHighlightingDataProperty); }
            set { SetValue(SyntaxHighlightingDataProperty, value); }
        }

        public static readonly PerspexProperty<Brush> PunctuationBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("PunctuationBrush");

        public Brush PunctuationBrush
        {
            get { return GetValue(PunctuationBrushProperty); }
            set { SetValue(PunctuationBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> KeywordBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("KeywordBrush");

        public Brush KeywordBrush
        {
            get { return GetValue(KeywordBrushProperty); }
            set { SetValue(KeywordBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> IdentifierBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("IdentifierBrush");

        public Brush IdentifierBrush
        {
            get { return GetValue(IdentifierBrushProperty); }
            set { SetValue(IdentifierBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> LiteralBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("LiteralBrush");

        public Brush LiteralBrush
        {
            get { return GetValue(LiteralBrushProperty); }
            set { SetValue(LiteralBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> UserTypeBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("UserTypeBrush");

        public Brush UserTypeBrush
        {
            get { return GetValue(UserTypeBrushProperty); }
            set { SetValue(UserTypeBrushProperty, value); }
        }

        public static readonly PerspexProperty<Brush> CommentBrushProperty =
            PerspexProperty.Register<TextEditor, Brush>("CommentBrush");

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

        public TextColoringTransformer TextColorizer { get; set; }

        public TextView TextView
        {
            get
            {
                return textView;
            }
        }
        #endregion

        #region Private Methods
       
       
        #endregion

        #region Public Methods
        public void InstallMargin(Control margin)
        {
            marginsContainer.Children.Add(margin);
        }
        #endregion

        #region Overrides
        protected override void OnTemplateApplied(INameScope nameScope)
        {
            textView = nameScope.Find<AvalonStudio.TextEditor.Rendering.TextView>("textView");
            textView.Cursor = new Cursor(StandardCursorType.Ibeam);

            TextView.BackgroundRenderers.Add(new SelectedLineBackgroundRenderer());
            TextView.DocumentLineTransformers.Add(TextColorizer);

            marginsContainer = nameScope.Find<StackPanel>("marginContainer");

            InstallMargin(new BreakPointMargin());
            InstallMargin(new LineNumberMargin());


            ScrollViewer = nameScope.Find<ScrollViewer>("scrollViewer");
        }

        public ScrollViewer ScrollViewer { get; set; }

       

        public void LineLeft()
        {
            throw new NotImplementedException();
        }

        public void LineRight()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelLeft()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelRight()
        {
            throw new NotImplementedException();
        }

        public void PageLeft()
        {
            throw new NotImplementedException();
        }

        public void PageRight()
        {
            throw new NotImplementedException();
        }

        public void LineDown()
        {
            throw new NotImplementedException();
        }

        public void LineUp()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelDown()
        {
            throw new NotImplementedException();
        }

        public void MouseWheelUp()
        {
            throw new NotImplementedException();
        }

        public void PageDown()
        {
            throw new NotImplementedException();
        }

        public void PageUp()
        {
            throw new NotImplementedException();
        }

        public Rect MakeVisible(Visual visual, Rect rectangle)
        {
            throw new NotImplementedException();
        }

        private void CaretTimer_Tick(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
