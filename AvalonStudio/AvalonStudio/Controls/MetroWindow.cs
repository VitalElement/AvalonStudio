namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Styling;
    using System;
    using Perspex.Input;
    using Perspex.Controls.Primitives;
    using Perspex;

    public class MetroWindow : Window, IStyleable
    {
        public MetroWindow()
        {
            
        }

        Type IStyleable.StyleKey => typeof(MetroWindow);

        private Grid titleBar;
        private Button minimiseButton;
        private Button restoreButton;
        private Button closeButton;
        private Grid topHorizontalGrip;
        private Grid bottomHorizontalGrip;
        private Grid leftVerticalGrip;
        private Grid rightVerticalGrip;
        private Grid topLeftGrip;
        private Grid bottomLeftGrip;
        private Grid topRightGrip;
        private Grid bottomRightGrip;
        private Panel icon;

        public static readonly PerspexProperty<Control> TitleBarContentProperty =
            PerspexProperty.Register<MetroWindow, Control>(nameof(TitleBarContent));

        public Control TitleBarContent
        {
            get { return GetValue(TitleBarContentProperty); }
            set { SetValue(TitleBarContentProperty, value); }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (topHorizontalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.North);
            }
            else if (bottomHorizontalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.South);
            }
            else if (leftVerticalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.West);
            }
            else if (rightVerticalGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.East);
            }
            else if(topLeftGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.NorthWest);
            }
            else if(bottomLeftGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.SouthWest);
            }
            else if (topRightGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.NorthEast);
            }
            else if(bottomRightGrip.IsPointerOver)
            {
                BeginResizeDrag(WindowEdge.SouthEast);
            }
            else if (titleBar.IsPointerOver)
            {
                WindowState = WindowState.Normal;
                BeginMoveDrag();
            }

            base.OnPointerPressed(e);
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {            
            titleBar = e.NameScope.Find<Grid>("titlebar");
            minimiseButton = e.NameScope.Find<Button>("minimiseButton");
            restoreButton = e.NameScope.Find<Button>("restoreButton");
            closeButton = e.NameScope.Find<Button>("closeButton");
            icon = e.NameScope.Find<Panel>("icon");

            topHorizontalGrip = e.NameScope.Find<Grid>("topHorizontalGrip");
            bottomHorizontalGrip = e.NameScope.Find<Grid>("bottomHorizontalGrip");
            leftVerticalGrip = e.NameScope.Find<Grid>("leftVerticalGrip");
            rightVerticalGrip = e.NameScope.Find<Grid>("rightVerticalGrip");

            topLeftGrip = e.NameScope.Find<Grid>("topLeftGrip");
            bottomLeftGrip = e.NameScope.Find<Grid>("bottomLeftGrip");
            topRightGrip = e.NameScope.Find<Grid>("topRightGrip");
            bottomRightGrip = e.NameScope.Find<Grid>("bottomRightGrip");

            minimiseButton.Click += (sender, ee) =>
            {
                WindowState = WindowState.Minimized;
            };

            restoreButton.Click += (sender, ee) =>
            {
                switch (WindowState)
                {
                    case WindowState.Maximized:
                        WindowState = WindowState.Normal;                        
                        break;

                    case WindowState.Normal:
                        WindowState = WindowState.Maximized;
                        break;
                }
            };

            closeButton.Click += (sender, ee) =>
            {
                Close();
            };

            icon.DoubleTapped += (sender, ee) =>
            {
                Close();
            };
        }
    }
}
