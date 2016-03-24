namespace AvalonStudio.Behaviors
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Primitives;
    using Perspex.Input;
    using Perspex.Metadata;
    using Perspex.Threading;
    using Perspex.Xaml.Interactivity;
    using System;

    public class PopupBehavior : Behavior<Control>
    {
        private DispatcherTimer timer;
        private Popup popup;
        protected Point lastPoint;

        public PopupBehavior()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            timer.Tick += Timer_Tick;

            popup = new Popup
            {
                PlacementMode = PlacementMode.Pointer,
                StaysOpen = false,
                Child = Content,
            };            

            ContentProperty.Changed.Subscribe((o) =>
            {
                popup.PlacementTarget = AssociatedObject;
                popup.Child = o.NewValue as Control;
            });
        }

        public static readonly PerspexProperty ContentProperty = PerspexProperty.Register<PopupBehavior, Control>(nameof(Content));

        [Content]
        public Control Content
        {
            get { return (Control)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.PointerLeave += AssociatedObject_PointerLeave;
            AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PointerLeave -= AssociatedObject_PointerLeave;
            AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
        }

        public static double GetDistanceBetweenPoints(Point p, Point q)
        {
            double a = p.X - q.X;
            double b = p.Y - q.Y;
            double distance = Math.Sqrt(a * a + b * b);
            return distance;
        }

        private void AssociatedObject_PointerMoved(object sender, PointerEventArgs e)
        {
            if (popup.IsOpen)
            {
                if (!popup.IsPointerOver)
                {
                    var distance = GetDistanceBetweenPoints(e.GetPosition(AssociatedObject), lastPoint);

                    if(distance > 28)
                    {
                        popup.Close();
                    }
                }
            }
            else
            {
                lastPoint = e.GetPosition(AssociatedObject);
                timer.Stop();
                timer.Start();                
            }
        }

        private void AssociatedObject_PointerLeave(object sender, PointerEventArgs e)
        {
            if (popup.IsOpen)
            {
                popup.Close();
            }
        }

        private void AssociatedObject_PointerEnter(object sender, PointerEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (OnBeforePopupOpen())
            {
                popup.Open();
            }
        }

        /// <summary>
        /// Method is called before popup opens to retrieve data and cancel popup open if required.
        /// </summary>
        /// <returns>true if the popup will open, false if it wont.</returns>
        public virtual bool OnBeforePopupOpen()
        {
            return true;
        }
    }
}

