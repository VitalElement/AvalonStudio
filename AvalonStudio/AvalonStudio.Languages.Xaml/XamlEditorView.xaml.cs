using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Remote;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Designer;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Diagnostics;
using System.Net;

namespace AvalonStudio.Languages.Xaml
{
    public class Center : Decorator
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Child != null)
            {
                var desired = Child.DesiredSize;
                Child.Arrange(new Rect((finalSize.Width - desired.Width) / 2, (finalSize.Height - desired.Height) / 2,
                    desired.Width, desired.Height));
            }
            return finalSize;
        }
    }

    public class XamlEditorView : UserControl
    {
        private IAvaloniaRemoteTransportConnection _connection;
        private RemoteWidget _remote;

        public XamlEditorView()
        {
            InitializeComponent();

            var editor = this.FindControl<AvalonStudio.Controls.Standard.CodeEditor.CodeEditor>("editor");

            editor.TextChanged += (sender, e) =>
            {
                _connection?.Send(new UpdateXamlMessage
                {
                    Xaml = editor.Document.Text
                });
            };

            var scroll = this.FindControl<ScrollViewer>("Remote");

            var rem = new Center();
            scroll.Content = rem;

            new BsonTcpTransport().Listen(IPAddress.Loopback, 12345, t =>
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection.OnMessage -= OnMessage;
                    }

                    _connection = t;

                    rem.Child = _remote = new RemoteWidget(t);

                    t.Send(new UpdateXamlMessage
                    {
                        Xaml = editor.Document.Text
                    });

                    t.OnMessage += OnMessage;
                });

            });

            var executingDir = Platforms.Platform.ExecutionPath;
            var args = $@"exec --runtimeconfig C:\dev\repos\GBSDFlashTool\FlashTool\bin\Debug\netcoreapp2.0\FlashTool.runtimeconfig.json --depsfile C:\dev\repos\GBSDFlashTool\FlashTool\bin\Debug\netcoreapp2.0\FlashTool.deps.json {executingDir}\HostApp\Avalonia.Designer.HostApp.dll --transport tcp-bson://127.0.0.1:12345/ C:\dev\repos\GBSDFlashTool\FlashTool\bin\Debug\netcoreapp2.0\FlashTool.dll";
            var process = Process.Start("dotnet", args);
        }

        private void OnMessage(IAvaloniaRemoteTransportConnection transport, object obj)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (transport != _connection)
                    return;
                if (obj is UpdateXamlResultMessage result)
                {
                    //_errorsContainer.IsVisible = result.Error != null;
                    //_errors.Text = result.Error ?? "";
                }
                if (obj is RequestViewportResizeMessage resize)
                {
                    _remote.Width = Math.Min(4096, Math.Max(resize.Width, 1));
                    _remote.Height = Math.Min(4096, Math.Max(resize.Height, 1));
                }
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}