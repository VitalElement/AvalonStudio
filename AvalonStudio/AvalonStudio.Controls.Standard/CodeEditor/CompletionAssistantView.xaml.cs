using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class CompletionAssistantView : TemplatedControl
    {
        private Popup _popup;
        private Point _lastPoint;

        public Control PlacementTarget { get; set; }

        public CompletionAssistantView()
        {
        }
        
        public void SetLocation(Point p)
        {
           if (_popup != null && PlacementTarget != null)
            {
                if(p != _lastPoint)
                {
                    _popup.HorizontalOffset = (-PlacementTarget.Bounds.Width) + p.X;
                    _popup.VerticalOffset = p.Y;
                    _popup.Open();

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
        }
    }
}