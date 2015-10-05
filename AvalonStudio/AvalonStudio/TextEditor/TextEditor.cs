namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Media;
    using Perspex.Styling;

    public class TextEditor : TemplatedControl
    {
        static TextEditor()
        {

        }

        public TextEditor()
        {

        }

        public static readonly PerspexProperty<string> TextProperty = PerspexProperty.Register<TextEditor, string>("Text");

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }

    /// <summary>
    /// The default style for the <see cref="TextEditor"/> control.
    /// </summary>
    public class TextEditorStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorStyle"/> class.
        /// </summary>
        public TextEditorStyle()
        {
            AddRange(new[]
            {
                new Style(x => x.OfType<TextEditor>())
                {
                    Setters = new[]
                    {
                        new Setter(TemplatedControl.TemplateProperty, new ControlTemplate<TextEditor>(Template)),
                        new Setter(TemplatedControl.BackgroundProperty, Brushes.White),
                        new Setter(TemplatedControl.BorderBrushProperty, new SolidColorBrush(0xff707070)),
                        new Setter(TemplatedControl.BorderThicknessProperty, 2.0),
                        new Setter(Control.FocusAdornerProperty, null),
                    },
                },
                new Style(x => x.OfType<TextEditor>().Class(":focus").Template().Name("border"))
                {
                    Setters = new[]
                    {
                        new Setter(TemplatedControl.BorderBrushProperty, Brushes.Black),
                    },
                }
            });
        }

        /// <summary>
        /// The default template for the <see cref="TextEditor"/> control.
        /// </summary>
        /// <param name="control">The control being styled.</param>
        /// <returns>The root of the instantiated template.</returns>
        public static Control Template(TextEditor control)
        {
            Border result = new Border
            {
                Name = "border",
                Padding = new Thickness(2),

                Child = new ScrollViewer
                {
                    [~ScrollViewer.CanScrollHorizontallyProperty] = control[~ScrollViewer.CanScrollHorizontallyProperty],
                    [~ScrollViewer.HorizontalScrollBarVisibilityProperty] = control[~ScrollViewer.HorizontalScrollBarVisibilityProperty],
                    [~ScrollViewer.VerticalScrollBarVisibilityProperty] = control[~ScrollViewer.VerticalScrollBarVisibilityProperty],
                    Content = new TextView
                    {
                        Name = "textPresenter",
                        [~TextView.TextProperty] = control[~TextEditor.TextProperty],
                    }
                }
            };

            return result;
        }
    }
}
