using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Utils;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Editor
{
    public class CodeEditorToolTip : TemplatedControl
    {
        private Popup _popup;

        private CodeEditor _editor;
        private Point _lastPoint;
        private readonly DispatcherTimer _timer;
        private Control _viewHost;

        public Control PlacementTarget { get; set; }

        public CodeEditorToolTip()
        {
            _timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 250)
            };

            _timer.Tick += _timer_Tick;
        }

        public void AttachEditor(CodeEditor editor)
        {
            _editor = editor;

            _editor.PointerMoved += _editor_PointerMoved;
        }

        private async void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();

            var editor = _editor;

            if (editor?.Document != null)
            {
                var dataContext = await OnBeforePopupOpen();

                if (dataContext != null)
                {
                    if (editor.IsPointerOver)
                    {
                        _viewHost.DataContext = dataContext;
                        var mouseDevice = (VisualRoot as IInputRoot)?.MouseDevice;
                        _lastPoint = mouseDevice.GetPosition(editor);

                        // adjust offset so popup is always a little bit below the line queried.
                        var translated = editor.TranslatePoint(_lastPoint, editor.TextArea.TextView);
                        var delta = (translated.Y % editor.TextArea.TextView.DefaultLineHeight);

                        _popup.VerticalOffset = (editor.TextArea.TextView.DefaultLineHeight - delta) + 1;

                        _popup.Open();
                    }
                }
                else
                {
                    _viewHost.DataContext = null;
                }
            }
        }

        private void _editor_PointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if (_popup.IsOpen)
            {
                var distance = e.GetPosition(_editor).DistanceTo(_lastPoint);

                if (distance > 25 && !_popup.PopupRoot.IsPointerOver)
                {
                    _popup.Close();
                }
            }
            else
            {
                var newPoint = e.GetPosition(_editor);

                if (newPoint != _lastPoint)
                {
                    _timer.Stop();
                    _timer.Start();
                }

                _lastPoint = newPoint;
            }
        }

        /// <summary>
        ///     Method is called before popup opens to retrieve data and cancel popup open if required.
        /// </summary>
        /// <returns>true if the popup will open, false if it wont.</returns>
        public async Task<object> OnBeforePopupOpen()
        {
            return await _editor.UpdateToolTipAsync();
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _viewHost = e.NameScope.Find<Control>("PART_Presenter");

            _popup = e.NameScope.Find<Popup>("PART_Popup");

            _popup.PlacementMode = PlacementMode.Pointer;

            _popup.HorizontalOffset = 0;
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _editor.PointerMoved -= _editor_PointerMoved;

            _popup.Close();
            _popup = null;
            _editor = null;
        }
    }
}