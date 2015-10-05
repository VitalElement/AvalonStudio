namespace AvalonStudio.TextEditor
{
    using System;
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Input;
    using Perspex.Media;

    public class TextView : Control, IScrollInfo
    {
        public TextView()
        {
            Text = "Not Clicked";

        }

        public static readonly PerspexProperty<string> TextProperty = PerspexProperty.Register<TextView, string>("Text", null, false, BindingMode.TwoWay);

        protected override void OnPointerPressed(PointerPressEventArgs e)
        {

        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            double y = e.Device.GetPosition(this).Y;
            y /= lineHeight;
            InvalidateVisual();
        }

        double lineHeight = 20;
        public override void Render(IDrawingContext context)
        {
            var textSize = new FormattedText("X", "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size };
            lineHeight = textSize.Measure().Height;

            if (Text != null)
            {
                var text = new FormattedText(Text, "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size };

                var renderSize = new Rect(Bounds.Size);

                context.FillRectangle(Brush.Parse("#1e1e1e"), renderSize);

                var lines = (int)(Bounds.Height / lineHeight);

                context.FillRectangle(Brush.Parse("#242424"), new Rect(Bounds.X + 14, (SelectedLine * lineHeight), Bounds.Width, lineHeight), 4);
                context.FillRectangle(Brush.Parse("#0f0f0f"), new Rect(Bounds.X + 14 + 2, (SelectedLine * lineHeight) + 2, Bounds.Width - 4, lineHeight - 4), 4);

                for (int i = 0; i < lines; i++)
                {
                    context.DrawText(Brushes.WhiteSmoke, new Point(0, 4 + (i * lineHeight)), new FormattedText((i + 1).ToString(), "Consolas", 12, FontStyle.Normal, TextAlignment.Left, FontWeight.Normal) { Constraint = Bounds.Size });
                }

                text.SetForegroundBrush(Brushes.Red, 50, 20);
                text.SetForegroundBrush(Brushes.Pink, 10, 5);
                text.SetForegroundBrush(Brushes.Green, 200, 500);

                
                context.DrawText(Brushes.White, new Point(18, 4), text);

                
            }
        }

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

        private int selectedLine;
        public int SelectedLine
        {
            get { return selectedLine; }
            set { selectedLine = value; }
        }

        public string Text
        {
            get { return GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public double ExtentWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double ViewportWidth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double HorizontalOffset
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool CanHorizontallyScroll
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double VerticalOffset
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public double ExtentHeight
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public double ViewportHeight
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanVerticallyScroll
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ScrollViewer ScrollOwner
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
