using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.LogicalTree;
using System;

namespace AvalonStudio.Controls.Editor
{
    public class CompletionAssistantView : TemplatedControl
    {
        private Popup _popup;
        private Point _lastPoint;

        public Control PlacementTarget { get; set; }

        public event EventHandler Closed;

        public CompletionAssistantView()
        {
        }

        public Popup Popup => _popup;

        public void SetLocation(Point p)
        {
            if (_popup != null && PlacementTarget != null)
            {
                if (p != _lastPoint)
                {
                    _popup.HorizontalOffset = (-PlacementTarget.Bounds.Width) + p.X;
                    _popup.VerticalOffset = p.Y;
                    _popup.Open(); // trigger move of popup.

                    _lastPoint = p;
                }
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _popup = e.NameScope.Find<Popup>("PART_Popup");

            _popup.PlacementTarget = PlacementTarget;
            _popup.PlacementMode = PlacementMode.Right;

            _popup.Closed += _popup_Closed;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _popup.Closed -= _popup_Closed;

            _popup.Close();

            _popup = null;
        }

        private void _popup_Closed(object sender, EventArgs e)
        {
            Closed?.Invoke(this, e);
        }
    }
}