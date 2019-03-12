using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls.Editor
{
    public class Intellisense : TemplatedControl
    {
        private Popup _popup;
        private Popup _assistantPopup;
        private Control _intellisense;
        private CompletionAssistantView _signatureHelper;

        public Control PlacementTarget { get; set; }

        public void SetSignatureHelper (CompletionAssistantView signatureHelper)
        {
            _signatureHelper = signatureHelper;

            _signatureHelper.Closed += _signatureHelper_Closed;
        }

        private void _signatureHelper_Closed(object sender, System.EventArgs e)
        {
            _popup.VerticalOffset -= _signatureHelper.Popup.PopupRoot.Bounds.Height;
            _popup.Open();
        }

        public Intellisense()
        {
        }

        static Intellisense()
        {
            RequestBringIntoViewEvent.AddClassHandler<Intellisense>(i => i.OnRequesteBringIntoView);
        }

        public void SetLocation(Point p, bool force = false)
        {
            if (_popup != null && PlacementTarget != null && (!_popup.IsOpen || force))
            {
                double verticalOffset = 0;

                if(_signatureHelper != null && _signatureHelper.Popup.IsOpen)
                {
                    verticalOffset += _signatureHelper.Popup.PopupRoot.Bounds.Height;
                }

                _popup.HorizontalOffset = (-PlacementTarget.Bounds.Width) + p.X;
                _popup.VerticalOffset = p.Y + 3 + verticalOffset;
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
            _assistantPopup.HorizontalOffset = 2;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _popup.Close();

            _signatureHelper = null;

            base.OnDetachedFromVisualTree(e);
        }

        private void OnRequesteBringIntoView(RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}