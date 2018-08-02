using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using AvalonStudio.Documents;
using System.Reactive.Disposables;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class RenameControl : TemplatedControl
    {
        private readonly CompositeDisposable disposables;
        private Popup _popup;
        private TextBox _textBox;

        public Control PlacementTarget { get; set; }
        private CodeEditor _editor;

        public RenameControl()
        {
            disposables = new CompositeDisposable();
        }

        public void Open(CodeEditor editor, string text)
        {
            _editor = editor;
            _textBox.AcceptsReturn = false;
            _textBox.Text = text;
            _textBox.SelectionStart = text.Length;
            _textBox.SelectionEnd = 0;
            _popup.Open();
            _textBox.Focus();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Escape)
            {
                if (e.Key == Key.Enter)
                {
                    _editor.RenameText = _textBox.Text;
                }

                PlacementTarget.Focus();
            }
        }

        public void SetLocation(Point p, bool force = false)
        {
            if (_popup != null && PlacementTarget != null && (!_popup.IsOpen || force))
            {
                double verticalOffset = 0;

                _popup.HorizontalOffset = (-PlacementTarget.Bounds.Width) + p.X;
                _popup.VerticalOffset = p.Y + 3 + verticalOffset;
            }
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _popup = e.NameScope.Find<Popup>("PART_Popup");
            _textBox = e.NameScope.Find<TextBox>("PART_TextBox");

            _popup.PlacementTarget = PlacementTarget;
            _popup.PlacementMode = PlacementMode.Right;
            _popup.StaysOpen = true;

            _popup.LostFocus += (sender, _) =>
            {
                _editor = null;
                _popup.Close();
                PlacementTarget.Focus();
            };
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            disposables.Add(RequestBringIntoViewEvent.AddClassHandler<Intellisense>(i => OnRequesteBringIntoView));

            base.OnAttachedToVisualTree(e);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _popup.Close();

            disposables.Dispose();

            base.OnDetachedFromVisualTree(e);
        }

        private void OnRequesteBringIntoView(RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}