namespace AvalonStudio.Shell.Behaviors
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Primitives;
    using Avalonia.Input;
    using Avalonia.LogicalTree;
    using Avalonia.Media;
    using Avalonia.Metadata;
    using Avalonia.Threading;
    using Avalonia.VisualTree;
    using Avalonia.Xaml.Interactivity;
    using AvalonStudio.Utils;
    using System;
    using System.Reactive.Disposables;
    using System.Threading.Tasks;

    public class TooltipBehavior : Behavior<Control>
    {
        public static readonly AvaloniaProperty ContentProperty =
            AvaloniaProperty.Register<TooltipBehavior, Control>(nameof(Content));

        private readonly CompositeDisposable disposables;
        private Point lastPoint;
        private Popup popup;
        private readonly DispatcherTimer timer;

        public TooltipBehavior()
        {
            disposables = new CompositeDisposable();

            timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 250)
            };

            timer.Tick += Timer_Tick;

            popup = new Popup
            {
                HorizontalOffset = 10,
                VerticalOffset = 10,
                PlacementMode = PlacementMode.Pointer,
                StaysOpen = false
            };
        }

        [Content]
        public Control Content
        {
            get { return (Control)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        private void Popup_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            popup.Close();
        }

        private void Popup_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            popup.Close();
        }

        protected override void OnAttached()
        {
            popup.PointerWheelChanged += Popup_PointerWheelChanged;
            popup.PointerPressed += Popup_PointerPressed;

            disposables.Add(ContentProperty.Changed.Subscribe(o =>
            {
                if (AssociatedObject != null && popup.PlacementTarget == null)
                {
                    popup.PlacementTarget = AssociatedObject as AvaloniaEdit.TextEditor;
                    popup.Child = new Grid
                    {
                        Children = { o.NewValue as Control },
                        Background = Brushes.Transparent
                    };
                }
            }));

            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree += AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged += AssociatedObject_PointerWheelChanged;
            AssociatedObject.DetachedFromLogicalTree += AssociatedObject_DetachedFromLogicalTree;
            AssociatedObject.DetachedFromVisualTree += AssociatedObject_DetachedFromVisualTree;
        }

        private void AssociatedObject_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            DetachObject();
        }

        private void AssociatedObject_DetachedFromLogicalTree(object sender, LogicalTreeAttachmentEventArgs e)
        {
            DetachObject();
        }

        private void DetachObject()
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
            popup.Close();
            popup.PointerWheelChanged -= Popup_PointerWheelChanged;
            popup.PointerPressed -= Popup_PointerPressed;
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
            AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
            AssociatedObject.AttachedToLogicalTree -= AssociatedObject_AttachedToLogicalTree;
            AssociatedObject.PointerWheelChanged -= AssociatedObject_PointerWheelChanged;
            AssociatedObject.DetachedFromLogicalTree -= AssociatedObject_DetachedFromLogicalTree;
            AssociatedObject.DetachedFromVisualTree -= AssociatedObject_DetachedFromVisualTree;
            disposables.Dispose();
        }

        private void AssociatedObject_AttachedToLogicalTree(object sender, LogicalTreeAttachmentEventArgs e)
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
                    var mouseDevice = (popup.PopupRoot.GetVisualRoot() as IInputRoot)?.MouseDevice;
                    lastPoint = mouseDevice.GetPosition(AssociatedObject);
                    popup.Open();
                }
            }
        }

        /// <summary>
        ///     Method is called before popup opens to retrieve data and cancel popup open if required.
        /// </summary>
        /// <returns>true if the popup will open, false if it wont.</returns>
        public virtual Task<bool> OnBeforePopupOpen()
        {
            return Task.FromResult(true);
        }
    }
}