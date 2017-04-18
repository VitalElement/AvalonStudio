using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Styling;
using System;

namespace AvalonStudio.Controls
{
    public class SplashScreen : Window, IStyleable
    {
        Type IStyleable.StyleKey => typeof(SplashScreen);
        private bool _mouseDown = false;

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);

            if (IsPointerOver && _mouseDown)
            {
                BeginMoveDrag();
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            _mouseDown = true;
            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerEventArgs e)
        {
            _mouseDown = false;
            base.OnPointerReleased(e);
        }
    }
}