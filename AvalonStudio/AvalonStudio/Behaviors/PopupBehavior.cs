namespace AvalonStudio.Behaviors
{
    using Perspex;
    using Perspex.Controls;
    using Perspex.Controls.Presenters;
    using Perspex.Controls.Primitives;
    using Perspex.Input;
    using Perspex.Media;
    using Perspex.Metadata;
    using Perspex.Threading;
    using Perspex.Xaml.Interactivity;
    using System;
    using Utils;

    public class PopupBehavior : Behavior<Control>
    {
        private DispatcherTimer timer;
        private Popup popup;
        protected Point lastPoint;

        public PopupBehavior()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Tick += Timer_Tick;

            popup = new Popup
            {
                PlacementMode = PlacementMode.Pointer,
                StaysOpen = false                              
            };

            popup.PointerWheelChanged += Popup_PointerWheelChanged;
            popup.PointerPressed += Popup_PointerPressed;

            ContentProperty.Changed.Subscribe((o) =>
            {   if (AssociatedObject != null)
                {
                    popup.PlacementTarget = (AssociatedObject as TextEditor.TextEditor).TextView;
                    popup.Child = new Grid() { Children = new Controls() { o.NewValue as Control }, Background = Brushes.Transparent };
                }           
            });
        }

        private void Popup_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            popup.Close();
        }

        private void Popup_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            popup.Close();
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
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree += AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged += AssociatedObject_PointerWheelChanged;
        }

        private void AssociatedObject_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            popup.Close();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree -= AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged -= AssociatedObject_PointerWheelChanged;
        }

        private void AssociatedObject_AttachedToLogicalTree(object sender, Perspex.LogicalTree.LogicalTreeAttachmentEventArgs e)
        {
            ((ISetLogicalParent)popup).SetParent(AssociatedObject);
        }        
                

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            popup.Close();
        }

        private void AssociatedObject_PointerMoved(object sender, PointerEventArgs e)
        {
            if (popup.IsOpen)
            {
                var distance = e.GetPosition(AssociatedObject).DistanceTo(lastPoint);

                if (distance > 14)
                {
                    popup.Close();
                }
            }
            else
            {
                var newPoint = e.GetPosition((AssociatedObject as TextEditor.TextEditor).TextView.TextSurface);

                if (newPoint != lastPoint)
                {
                    timer.Stop();
                    timer.Start();
                }

                lastPoint = newPoint;
            }
        }
        

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (OnBeforePopupOpen())
            {
                if (AssociatedObject.IsPointerOver)
                {
                    popup.Open();
                }
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

