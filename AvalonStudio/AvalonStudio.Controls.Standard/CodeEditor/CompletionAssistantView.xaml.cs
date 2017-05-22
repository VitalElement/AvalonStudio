using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class CompletionAssistantView : TemplatedControl
    {
        private Popup _popup;

        public Control PlacementTarget { get; set; }

        public CompletionAssistantView()
        {
        }

        public void SetLocation(Point p)
        {
           if (_popup != null && PlacementTarget != null && !_popup.IsOpen)
            {
                _popup.HorizontalOffset = (-PlacementTarget.Bounds.Width) + p.X;
                _popup.VerticalOffset = p.Y;
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