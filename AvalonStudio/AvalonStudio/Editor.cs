namespace AvalonStudio
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Presenters;
    using Perspex.Controls.Primitives;
    using Perspex.Controls.Templates;
    using Perspex.Media;
    using Perspex.Styling;
    using System.Reactive.Linq;
    using System;

    public class Editor : TemplatedControl
    {
        public override void Render(DrawingContext context)        
        {            
            Brush background = Brushes.Black;

            if (background != null)
            {
                context.FillRectangle(background, Bounds, 0);
            }

            base.Render(context);

            //context.DrawText(Brushes.White, new Perspex.Point(), new FormattedText("Testing\r\nTesting", "Consolas", 14, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size });
        }        

        //public class EditorStyle : Styles
        //{
        //    /// <summary>
        //    /// Initializes a new instance of the <see cref="TextBoxStyle"/> class.
        //    /// </summary>
        //    public EditorStyle()
        //    {
        //        AddRange(new[]
        //        {
        //        new Style(x => x.OfType<Editor>())
        //        {
        //            Setters = new[]
        //            {
        //                new Setter(TemplatedControl.TemplateProperty, new ControlTemplate<Editor>(Template)),
        //                new Setter(TemplatedControl.BackgroundProperty, Brushes.Red),
        //                new Setter(TemplatedControl.BorderBrushProperty, new SolidColorBrush(0xff707070)),
        //                new Setter(TemplatedControl.BorderThicknessProperty, 2.0),
        //                new Setter(Control.FocusAdornerProperty, null),
        //            },
        //        },
        //        new Style(x => x.OfType<Editor>().Class(":focus").Template().Name("border"))
        //        {
        //            Setters = new[]
        //            {
        //                new Setter(TemplatedControl.BorderBrushProperty, Brushes.Black),
        //            },
        //        }
        //    });
        //    }
        //}


        /// <summary>
        /// The default template for the <see cref="TextBox"/> control.
        /// </summary>
        /// <param name="control">The control being styled.</param>
        /// <returns>The root of the instantiated template.</returns>
        //public static Control Template(Editor control)
        //{
        //    Border result = new Border
        //    {
        //        Name = "border",
        //        Padding = new Thickness(2),
        //        [~Border.BackgroundProperty] = control[~TemplatedControl.BackgroundProperty],
        //        [~Border.BorderBrushProperty] = control[~TemplatedControl.BorderBrushProperty],
        //        [~Border.BorderThicknessProperty] = control[~TemplatedControl.BorderThicknessProperty],

        //        Child = new StackPanel
        //        {
        //            Children = new Perspex.Controls.Controls
        //            {
        //                new Panel
        //                {
        //                    Children = new Perspex.Controls.Controls
        //                    {                                
        //                        new ScrollViewer
        //                        {
        //                            [~ScrollViewer.CanScrollHorizontallyProperty] = control[~ScrollViewer.CanScrollHorizontallyProperty],
        //                            [~ScrollViewer.HorizontalScrollBarVisibilityProperty] = control[~ScrollViewer.HorizontalScrollBarVisibilityProperty],
        //                            [~ScrollViewer.VerticalScrollBarVisibilityProperty] = control[~ScrollViewer.VerticalScrollBarVisibilityProperty],
        //                            Content = new TextPresenter
        //                            {
        //                                Name = "textPresenter",

        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        },
        //    };

        //    return result;
        //}
    }
}
