namespace AvalonStudio.Behaviours
{
    using OmniXaml.Attributes;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Primitives;
    using Avalonia.Input;
    using Avalonia.Threading;
    using Perspex.Xaml.Interactivity;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a way to quickly configure a tooltip for arbitrary framework elements.
    /// </summary>
    [ContentProperty("Content")]    
    public class ToolTipBehavior : Behavior<Control>
    {
        private readonly DispatcherTimer timer;
        private Action timerAction;
        private Popup popup;
        private ToolTip toolTip;

        #region Content

        /// <summary>
        /// Content Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty ContentProperty = AvaloniaProperty.Register<ToolTipBehavior, object>(nameof(Content), string.Empty);

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }


        #endregion

        #region ContentStringFormat

        /// <summary>
        ///  ContentStringFormat Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty ContentStringFormatProperty = AvaloniaProperty.Register<ToolTipBehavior, string>(nameof(ContentStringFormat));        
        
        public string ContentStringFormat
        {
            get { return (string)GetValue(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }

        #endregion

        #region Header

        /// <summary>
        /// Header Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty TitleProperty = AvaloniaProperty.Register<ToolTipBehavior, object>(nameof(Header), string.Empty);
        
        public object Header
        {
            get { return GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region HeaderStringFormat

        /// <summary>
        /// HeaderStringFormat Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty TitleStringFormatProperty = AvaloniaProperty.Register<ToolTipBehavior, string>(nameof(TitleStringFormat));

        public string TitleStringFormat
        {
            get { return (string)GetValue(TitleStringFormatProperty); }
            set { SetValue(TitleStringFormatProperty, value); }
        }

        #endregion


        #region MaxWidth

        /// <summary>
        /// MaxWidth Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty MaxWidthProperty = AvaloniaProperty.Register<ToolTipBehavior, double>(nameof(MaxWidth));
        
        public double MaxWidth
        {
            get { return (double)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }

        #endregion

        #region Delay

        /* Keep in mind that changing the delay might mess with the animations */
        public static readonly AvaloniaProperty DelayProperty = AvaloniaProperty.Register<ToolTipBehavior, int>(nameof(Delay), 500, false, Avalonia.Data.BindingMode.TwoWay, ValidateDelay, OnDelayChanged);        

        private static int ValidateDelay(ToolTipBehavior behavoir, int value)
        {            
            var delay = (int)value;
            return delay >= 100 ? 0 : 1;
        }

        private static void OnDelayChanged(IAvaloniaObject sender, bool changed)
        {
            var b = (ToolTipBehavior)sender;
            b.timer.Interval = TimeSpan.FromMilliseconds(b.Delay);
        }
        
        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        #endregion

        #region IsInteractive

        public static readonly AvaloniaProperty IsInteractiveProperty = AvaloniaProperty.Register<ToolTipBehavior, bool>(nameof(IsInteractive), true, false, Avalonia.Data.BindingMode.OneWay, null, OnIsInteractiveChanged);
        
        public bool IsInteractive
        {
            get { return (bool)GetValue(IsInteractiveProperty); }
            set { SetValue(IsInteractiveProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsInteractive property.
        /// </summary>
        private static void OnIsInteractiveChanged(IAvaloniaObject d, bool changing)
        {
            var target = (ToolTipBehavior)d;
            if (target.toolTip == null) return;

            target.toolTip.IsHitTestVisible = target.IsInteractive;
        }

        #endregion

        #region IsEnabled

        /// <summary>
        /// IsEnabled Dependency Property
        /// </summary>
        public static readonly AvaloniaProperty IsEnabledProperty = AvaloniaProperty.Register<ToolTipBehavior, bool>(nameof(IsEnabled), true, false, Avalonia.Data.BindingMode.OneWay, null, OnIsEnabledChanged);        
        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        private static void OnIsEnabledChanged(IAvaloniaObject d, bool changed)
        {
            var behavior = (ToolTipBehavior)d;
            if (!behavior.IsEnabled)
            {
                //hide tooltip
                behavior.HideAndClose();
            }
        }

        #endregion

        
        public ToolTipBehavior()
        {            
            timer = new DispatcherTimer(TimeSpan.FromMilliseconds(Delay), DispatcherPriority.Normal, OnTimerElapsed);
            timer.IsEnabled = false;
        }


        /// <summary>
        /// Inits the helper popup and tooltip controls.
        /// </summary>
        private void InitControls()
        {
            popup = new Popup
            {
                PlacementMode= PlacementMode.Pointer, //note: set popup.PlacementTarget to the AssociatedObject, the tooltip will derive formatting                
            };

            popup.PointerEnter += OnPopupMouseEnter;
            popup.PointerLeave += OnPopupMouseLeave;

            ////hook up the popup with the data context of it's associated object
            ////in case we have content with bindings
            //var binding = new Binding
            //{
            //    Path = new PropertyPath(FrameworkElement.DataContextProperty),
            //    Mode = BindingMode.OneWay,
            //    Source = AssociatedObject
            //};

            //BindingOperations.SetBinding(popup, FrameworkElement.DataContextProperty, binding);

            //if the content is itself already a tooltip control, don't wrap it
            //-> any other properties will be ignored!
            toolTip = Content as ToolTip ?? CreateWrapperControl();

            popup.Child = toolTip;
            toolTip.IsHitTestVisible = IsInteractive;

            //force template application so we can switch visual states
            toolTip.ApplyTemplate();
            //VisualStateManager.GoToState(toolTip, "Closed", false);
        }


        /// <summary>
        /// Creates a <see cref="HeaderedToolTip"/> control and wires it's properties
        /// with the behavior through data binding.
        /// </summary>
        private ToolTip CreateWrapperControl()
        {
            //create wrapper tooltip control
            var tt = new ToolTip();

            //wire tooltip content
            CreateBinding(ContentProperty, ContentControl.ContentProperty, tt);            

            //wire tooltip title
            CreateBinding(TitleProperty, HeaderedContentControl.HeaderProperty, tt);                      

            //wire max width
            CreateBinding(MaxWidthProperty, Control.MaxWidthProperty, tt);

            return tt;
        }

        private void CreateBinding(AvaloniaProperty behaviorProperty, AvaloniaProperty targetProperty, IAvaloniaObject target)
        {
            //wire max width
            target.Bind(targetProperty, behaviorProperty.Bind());            
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PointerEnter += OnMouseEnter;
            AssociatedObject.PointerLeave += OnMouseLeave;
        }


        private void OnMouseEnter(object sender, PointerEventArgs e)
        {
            if (!IsEnabled) return;

            //if the popup is still open, open it immediatly
            if (popup != null && popup.IsOpen)
            {
                Schedule(() => { }); //reset schedule
                //VisualStateManager.GoToState(toolTip, "Showing", true);
            }
            else
            {
                Schedule(() =>
                {
                    popup.IsOpen = true; //show, then transition
                    //VisualStateManager.GoToState(toolTip, "Showing", true);
                });
            }
        }

        private void OnPopupMouseEnter(object sender, PointerEventArgs e)
        {
            timer.Stop(); //suppress any other actions
            //VisualStateManager.GoToState(toolTip, "Active", true); //the popup is still open

            //register an event handler for the deactivation event of the parent window
            //-> prevents the popup from remaining open if the window is being deactivated
            //var parentWindow = Window.GetWindow(AssociatedObject);
            //if (parentWindow != null)
            //{
            //    parentWindow.Deactivated += OnParentWindowDeactivated;
            //}
        }


        /// <summary>
        /// Closes the popup immediately, if the parent window was deactivated.
        /// Prevents the popup from hovering over other applications (popups
        /// are topmost).
        /// </summary>
        private void OnParentWindowDeactivated(object sender, EventArgs e)
        {
            //immediately close popup
            timer.Stop();

            var parentWindow = (Window)sender;
            parentWindow.Deactivated -= OnParentWindowDeactivated;

            popup.IsOpen = false;
            //VisualStateManager.GoToState(toolTip, "Closed", false);
        }

        private void OnMouseLeave(object sender, PointerEventArgs e)
        {
            HideAndClose();
        }

        /// <summary>
        /// Transitions the popup into a closed state.
        /// </summary>
        private void HideAndClose()
        {
            if (popup == null) return;

            //start fading immediately if animation is programmed that way
            //VisualStateManager.GoToState(toolTip, "Hiding", true);

            Schedule(() =>
            {
                //VisualStateManager.GoToState(toolTip, "Closed", true);
                popup.IsOpen = false;
            });
        }

        private void OnPopupMouseLeave(object sender, PointerEventArgs e)
        {
            //remove event handler for the deactivation event of the parent window
            //var parentWindow = Window.GetWindow(AssociatedObject);
            //if (parentWindow != null)
            //{
            //    parentWindow.Deactivated -= OnParentWindowDeactivated;
            //}

            //Schedule(() =>
            //{
            //    VisualStateManager.GoToState(toolTip, "Hiding", true); //switch with a delay when leaving the popup
            //    Schedule(() =>
            //    {
            //        popup.IsOpen = false;
            //    }); //close with yet another delay
            //});
        }


        /// <summary>
        /// Schedules an action to be executed on the next timer tick. Resets the timer
        /// and replaces any other pending action.
        /// </summary>
        /// <remarks>
        /// Customize this if you need custom delays to show / hide / fade tooltips by
        /// simply changing the timer interval depending on the state change.
        /// </remarks>
        private void Schedule(Action action)
        {
            lock (timer)
            {
                timer.Stop();
                if (popup == null) InitControls();

                timerAction = action;
                timer.Start();
            }
        }


        /// <summary>
        /// Performs a pending action.
        /// </summary>
        private void OnTimerElapsed(object sender, EventArgs eventArgs)
        {
            lock (timer)
            {
                timer.Stop();

                var action = timerAction;
                timerAction = null;

                //cache action and set timer action to null before invoking
                //(that action could cause the timeraction to be reassigned)
                if (action != null) action();
            }
        }
    }
}
