namespace AvalonStudio.Behaviors
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Presenters;
    using Avalonia.Controls.Primitives;
    using Avalonia.Input;
    using Avalonia.Media;
    using Avalonia.Metadata;
    using Avalonia.Threading;
    using Avalonia.Xaml.Interactivity;
    using System;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;
    using Utils;

    public class PopupBehavior : Behavior<Control>
    {
        private DispatcherTimer timer;
        private Popup popup;
        private Point lastPoint;
        private CompositeDisposable disposables;

        public PopupBehavior()
        {
            disposables = new CompositeDisposable();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 250);
            timer.Tick += Timer_Tick;

            popup = new Popup
            {
                HorizontalOffset = 10,
                VerticalOffset = 10,
                PlacementMode = PlacementMode.Pointer,
                StaysOpen = false
            };
        }

        private void Popup_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            popup.Close();
        }

        private void Popup_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            popup.Close();
        }

        public static readonly AvaloniaProperty ContentProperty = AvaloniaProperty.Register<PopupBehavior, Control>(nameof(Content));

        [Content]
        public Control Content
        {
            get { return (Control)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        protected override void OnAttached()
        {
            popup.PointerWheelChanged += Popup_PointerWheelChanged;
            popup.PointerPressed += Popup_PointerPressed;

            disposables.Add(ContentProperty.Changed.Subscribe((o) =>
            {
                if (AssociatedObject != null && popup.PlacementTarget == null)
                {
                    popup.PlacementTarget = (AssociatedObject as TextEditor.TextEditor);
                    popup.Child = new Grid() { Children = new Controls() { o.NewValue as Control }, Background = Brushes.Transparent };
                }
            }));

            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree += AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged += AssociatedObject_PointerWheelChanged;
            AssociatedObject.DetachedFromLogicalTree += AssociatedObject_DetachedFromLogicalTree;

        }

        private void AssociatedObject_DetachedFromLogicalTree(object sender, Avalonia.LogicalTree.LogicalTreeAttachmentEventArgs e)
        {
            OnDetaching();
            popup.Child = null;
            popup.PlacementTarget = null;
            ((ISetLogicalParent)popup).SetParent(null);
            popup = null;
        }

        private void AssociatedObject_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            popup.Close();
        }

        protected override void OnDetaching()
        {
            popup.PointerWheelChanged -= Popup_PointerWheelChanged;
            popup.PointerPressed -= Popup_PointerPressed;
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree -= AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged -= AssociatedObject_PointerWheelChanged;
            AssociatedObject.DetachedFromLogicalTree -= AssociatedObject_DetachedFromLogicalTree;
            disposables.Dispose();
        }

        private void AssociatedObject_AttachedToLogicalTree(object sender, Avalonia.LogicalTree.LogicalTreeAttachmentEventArgs e)
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
                var newPoint = e.GetPosition(AssociatedObject);

                if (newPoint != lastPoint)
                {
                    timer.Stop();
                    timer.Start();
                }

                lastPoint = newPoint;
            }
        }


        private async void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();

            if (await OnBeforePopupOpen())
            {
                if (AssociatedObject.IsPointerOver)
                {
                    lastPoint = MouseDevice.Instance.GetPosition(AssociatedObject);
                    popup.Open();
                }
            }
        }

        /// <summary>
        /// Method is called before popup opens to retrieve data and cancel popup open if required.
        /// </summary>
        /// <returns>true if the popup will open, false if it wont.</returns>
        public virtual async Task<bool> OnBeforePopupOpen()
        {
            return true;
        }
    }
}

