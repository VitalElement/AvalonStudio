using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class Intellisense : TemplatedControl
    {
        private readonly CompositeDisposable disposables;
        private Popup _popup;
        private Popup _assistantPopup;
        private Control _intellisense;

        public Control PlacementTarget { get; set; }

        public Intellisense()
        {
            disposables = new CompositeDisposable();
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
            _assistantPopup = e.NameScope.Find<Popup>("PART_PopupAssistant");
            _intellisense = e.NameScope.Find<Control>("PART_Intellisense");

            _popup.PlacementTarget = PlacementTarget;
            _popup.PlacementMode = PlacementMode.Right;
            _popup.StaysOpen = true;

            _assistantPopup.PlacementTarget = _intellisense;
            _assistantPopup.PlacementMode = PlacementMode.Right;
            _assistantPopup.StaysOpen = true;
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Add(RequestBringIntoViewEvent.AddClassHandler<Intellisense>(i => OnRequesteBringIntoView));
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Dispose();
        }

        private void OnRequesteBringIntoView(RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}