using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Utils;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class TooltipView : TemplatedControl
    {
        private Popup _popup;

        private CodeEditor _editor;
        private Point _lastPoint;
        private readonly DispatcherTimer _timer;
        private Control _viewHost;

        public Control PlacementTarget { get; set; }

        public TooltipView()
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

            if (_editor.Document != null)
            {
                var dataContext = await OnBeforePopupOpen();

                if (dataContext != null)
                {
                    if (_editor.IsPointerOver)
                    {
                        _viewHost.DataContext = dataContext;
                        _lastPoint = MouseDevice.Instance.GetPosition(_editor);
                        _popup.Open();
                    }
                }
                else
                {
                    DataContext = null;
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

            _viewHost = e.NameScope.Find<Control>("PART_ViewHost");

            _popup = e.NameScope.Find<Popup>("PART_Popup");

            _popup.PlacementMode = PlacementMode.Pointer;

            _popup.HorizontalOffset = 0;
            _popup.VerticalOffset = 0;
        }

        
    }
}