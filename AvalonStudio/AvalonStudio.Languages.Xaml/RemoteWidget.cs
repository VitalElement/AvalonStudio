using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using System;
using System.Runtime.InteropServices;
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

        public RemoteWidget(IAvaloniaRemoteTransportConnection connection)
        {
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

        }

        protected override void ArrangeCore(Rect finalRect)
        {
            _connection.Send(new ClientViewportAllocatedMessage
            {
                Width = finalRect.Width,
                Height = finalRect.Height,
                DpiX = 96,
                DpiY = 96 //TODO: Somehow detect the actual DPI
            });
            base.ArrangeCore(finalRect);
        }

        public override void Render(DrawingContext context)
        {
            if (_lastFrame != null)
            {
                var fmt = (PixelFormat)_lastFrame.Format;
                if (_bitmap == null || _bitmap.PixelWidth != _lastFrame.Width ||
                    _bitmap.PixelHeight != _lastFrame.Height)
                    _bitmap = new WriteableBitmap(_lastFrame.Width, _lastFrame.Height, fmt);
                using (var l = _bitmap.Lock())
                {
                    var lineLen = (fmt == PixelFormat.Rgb565 ? 2 : 4) * _lastFrame.Width;
                    for (var y = 0; y < _lastFrame.Height; y++)
                        Marshal.Copy(_lastFrame.Data, y * _lastFrame.Stride,
                            new IntPtr(l.Address.ToInt64() + l.RowBytes * y), lineLen);
                }
                context.DrawImage(_bitmap, 1, new Rect(0, 0, _bitmap.PixelWidth, _bitmap.PixelHeight),
                    new Rect(Bounds.Size));
            }
            base.Render(context);
        }
    }
}