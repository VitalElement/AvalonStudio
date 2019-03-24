using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Input;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using InputModifiers = Avalonia.Input.InputModifiers;
using PixelFormat = Avalonia.Platform.PixelFormat;

namespace AvalonStudio.Languages.Xaml
{
    public class RemoteWidget : Control
    {
        private readonly IAvaloniaRemoteTransportConnection _connection;
        private FrameMessage _lastFrame;
        private FrameMessage _lastErrorFrame;
        private WriteableBitmap _bitmap;
        private bool _inError;
        private double _width;
        private double _height;
        private double _zoomLevel;

        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = value;

                var scaling = GetScaling();

                _connection.Send(new ClientRenderInfoMessage
                {
                    DpiX = (96 * scaling) * ZoomLevel,
                    DpiY = (96 * scaling) * ZoomLevel
                });

                Width = Math.Min(4096, Math.Max(_width * ZoomLevel, 1));
                Height = Math.Min(4096, Math.Max(_height * ZoomLevel, 1));
            }
        }

        public RemoteWidget(IAvaloniaRemoteTransportConnection connection)
        {
            Focusable = true;
            _zoomLevel = 1;
            _connection = connection;
            _connection.OnMessage += (t, msg) => Dispatcher.UIThread.Post(() => OnMessage(msg));
            _connection.Send(new ClientSupportedPixelFormatsMessage
            {
                Formats = new[]
                {
                    Avalonia.Remote.Protocol.Viewport.PixelFormat.Bgra8888,
                    Avalonia.Remote.Protocol.Viewport.PixelFormat.Rgba8888,
                }
            });

            var scaling = GetScaling();

            _connection.Send(new ClientRenderInfoMessage
            {
                DpiX = (96 * scaling) * ZoomLevel,
                DpiY = (96 * scaling) * ZoomLevel
            });

            AddHandler(KeyUpEvent, OnKeyUp, Avalonia.Interactivity.RoutingStrategies.Tunnel);

            AddHandler(KeyDownEvent, OnKeyDown, Avalonia.Interactivity.RoutingStrategies.Tunnel);

            AddHandler(TextInputEvent, OnTextInput, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        }

        private double GetScaling ()
        {
            var window = (Window)VisualRoot;

            if (window == null)
            {
                window = Application.Current.MainWindow;
            }

            if (window != null)
            {
                return window.PlatformImpl.Scaling;
            }

            return 1;
        }

        public bool InError
        {
            get { return _inError; }
            set
            {
                _inError = value;

                if(!_inError && _lastErrorFrame != null)
                {
                    _lastFrame = _lastErrorFrame;
                    InvalidateVisual();
                    _lastErrorFrame = null;
                }
            }
        }

        private void OnMessage(object msg)
        {
            if (msg is FrameMessage frame)
            {
                _connection.Send(new FrameReceivedMessage
                {
                    SequenceId = frame.SequenceId
                });

                if (!InError)
                {
                    _lastFrame = frame;
                    InvalidateVisual();
                }
                else
                {
                    _lastErrorFrame = frame;
                }
            }

            if (msg is RequestViewportResizeMessage resize)
            {
                _width = resize.Width;
                _height = resize.Height;
                Width = Math.Min(4096, Math.Max(resize.Width * ZoomLevel, 1));
                Height = Math.Min(4096, Math.Max(resize.Height * ZoomLevel, 1));
            }
        }

        private static Avalonia.Remote.Protocol.Input.InputModifiers[] ToAvaloniaModifiers(InputModifiers modifiers)
        {
            var result = new List<Avalonia.Remote.Protocol.Input.InputModifiers>();

            if(modifiers.HasFlag(InputModifiers.Control))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Control);
            }
            
            if(modifiers.HasFlag(InputModifiers.Alt))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Alt);
            }

            if (modifiers.HasFlag(InputModifiers.Shift))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Shift);
            }

            if (modifiers.HasFlag(InputModifiers.Windows))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.Windows);
            }

            if (modifiers.HasFlag(InputModifiers.LeftMouseButton))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.LeftMouseButton);
            }

            if (modifiers.HasFlag(InputModifiers.RightMouseButton))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.RightMouseButton);
            }

            if (modifiers.HasFlag(InputModifiers.MiddleMouseButton))
            {
                result.Add(Avalonia.Remote.Protocol.Input.InputModifiers.MiddleMouseButton);
            }

            return result.ToArray();
        }

        private Point GetMousePoint(PointerEventArgs e)
        {
            return e.GetPosition(this) / ZoomLevel;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            var point = GetMousePoint(e);

            _connection.Send(new PointerPressedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                Button = (Avalonia.Remote.Protocol.Input.MouseButton)e.MouseButton
            });

            base.OnPointerPressed(e);
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            var point = GetMousePoint(e);

            _connection.Send(new PointerReleasedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                Button = (Avalonia.Remote.Protocol.Input.MouseButton)e.MouseButton
            });

            base.OnPointerReleased(e);
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            var point = GetMousePoint(e);

            _connection.Send(new ScrollEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y,
                DeltaX = e.Delta.X / ZoomLevel,
                DeltaY = e.Delta.Y / ZoomLevel
            });

            base.OnPointerWheelChanged(e);
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            var point = GetMousePoint(e);

            _connection.Send(new PointerMovedEventMessage
            {
                Modifiers = ToAvaloniaModifiers(e.InputModifiers),
                X = point.X,
                Y = point.Y
            });

            base.OnPointerMoved(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _connection.Send(new KeyEventMessage
            {
                 IsDown = true,
                 Key = (Avalonia.Remote.Protocol.Input.Key)e.Key,
                 Modifiers = ToAvaloniaModifiers(e.Modifiers)
            });

            e.Handled = true;

            base.OnKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            _connection.Send(new KeyEventMessage
            {
                IsDown = false,
                Key = (Avalonia.Remote.Protocol.Input.Key)e.Key,
                Modifiers = ToAvaloniaModifiers(e.Modifiers)
            });

            e.Handled = true;

            base.OnKeyUp(e);
        }

        private void OnTextInput(object sender, TextInputEventArgs e)
        {
            _connection.Send(new TextInputEventMessage
            {
                Text = e.Text
            });

            e.Handled = true;

            base.OnTextInput(e);
        }

        public override void Render(DrawingContext context)
        {
            if (_lastFrame != null && !(_lastFrame.Width == 0 || _lastFrame.Width == 0))
            {
                var fmt = (PixelFormat)_lastFrame.Format;
                if (_bitmap == null || _bitmap.Size.Width != _lastFrame.Width ||
                    _bitmap.Size.Height != _lastFrame.Height)
                    _bitmap = new WriteableBitmap(new PixelSize(_lastFrame.Width, _lastFrame.Height), new Vector(96,96), fmt);
                using (var l = _bitmap.Lock())
                {
                    var lineLen = (fmt == PixelFormat.Rgb565 ? 2 : 4) * _lastFrame.Width;
                    for (var y = 0; y < _lastFrame.Height; y++)
                        Marshal.Copy(_lastFrame.Data, y * _lastFrame.Stride,
                            new IntPtr(l.Address.ToInt64() + l.RowBytes * y), lineLen);
                }
                context.DrawImage(_bitmap, 1, new Rect(0, 0, _bitmap.Size.Width, _bitmap.Size.Height),
                    new Rect(Bounds.Size));
            }
            base.Render(context);
        }
    }
}