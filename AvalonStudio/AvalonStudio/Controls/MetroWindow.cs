namespace AvalonStudio.Controls
{
    using Perspex.Controls;
    using Perspex.Styling;
    using System;
    using Perspex.Input;

    public class MetroWindow : Window, IStyleable
    {
        public MetroWindow()
        {
        }

        Type IStyleable.StyleKey => typeof(MetroWindow);

        private Grid titleBar;
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

        protected override void OnPointerPressed(PointerPressEventArgs e)
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
                BeginMoveDrag();
            }

            base.OnPointerPressed(e);
        }

        protected override void OnTemplateApplied(INameScope nameScope)
        {
            titleBar = nameScope.Find<Grid>("titlebar");
            restoreButton = nameScope.Find<Button>("restoreButton");
            closeButton = nameScope.Find<Button>("closeButton");

            topHorizontalGrip = nameScope.Find<Grid>("topHorizontalGrip");
            bottomHorizontalGrip = nameScope.Find<Grid>("bottomHorizontalGrip");
            leftVerticalGrip = nameScope.Find<Grid>("leftVerticalGrip");
            rightVerticalGrip = nameScope.Find<Grid>("rightVerticalGrip");

            topLeftGrip = nameScope.Find<Grid>("topLeftGrip");
            bottomLeftGrip = nameScope.Find<Grid>("bottomLeftGrip");
            topRightGrip = nameScope.Find<Grid>("topRightGrip");
            bottomRightGrip = nameScope.Find<Grid>("bottomRightGrip");            

            restoreButton.Click += (sender, e) =>
            {
                
            };

            closeButton.Click += (sender, e) =>
            {
                Close();
            };

            base.OnTemplateApplied(nameScope);
        }
    }
}
