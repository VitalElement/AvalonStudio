namespace AvalonStudio.TextEditor
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Media;
    using Perspex.Styling;
    using System.Linq;
    using System.Reactive.Linq;

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
                        new Setter(TemplatedControl.BackgroundProperty, Brush.Parse ("#1e1e1e")),
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
            var textView = new TextView
            {
                Name = "textView",       
                HorizontalAlignment = Perspex.Layout.HorizontalAlignment.Stretch,         
                [~TextView.CaretIndexProperty] = control[~TextEditor.CaretIndexProperty],
                [~TextView.SelectionStartProperty] = control[~TextEditor.SelectionStartProperty],
                [~TextView.SelectionEndProperty] = control[~TextEditor.SelectionEndProperty],
                [~TextBlock.TextProperty] = control[~TextEditor.TextProperty],
                [~TextBlock.TextWrappingProperty] = control[~TextEditor.TextWrappingProperty],
                [Grid.ColumnProperty] = 2,
            };

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
                    Content = new Panel
                    {
                        Children = new Perspex.Controls.Controls
                        {                                                        
                            new Grid
                            {
                                ColumnDefinitions = new ColumnDefinitions
                                {
                                    new ColumnDefinition(1, GridUnitType.Auto),
                                    new ColumnDefinition(1, GridUnitType.Star),
                                },
                                Children = new Perspex.Controls.Controls
                                {
                                    new LineNumberMargin(textView)
                                    {
                                        Margin = new Thickness (0, 0, 10, 0),
                                        [Grid.ColumnSpanProperty] = 1,
                                        [~LineNumberMargin.HeightProperty] = control[~TemplatedControl.HeightProperty]
                                    },
                                    textView
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
