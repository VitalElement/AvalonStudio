using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Remote;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Designer;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using AvalonStudio.Extensibility;
using Avalonia.Data;
using AvalonStudio.Shell;

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

    public class AvaloniaPreviewer : TemplatedControl
    {
        private IAvaloniaRemoteTransportConnection _connection;
        private RemoteWidget _remote;
        private Center _remoteContainer;
        private Process _currentHost;

        private static int FreeTcpPort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public static readonly AvaloniaProperty<string> XamlProperty = AvaloniaProperty.Register<AvaloniaPreviewer, string>(nameof(Xaml));

        public string Xaml
        {
            get => GetValue(XamlProperty);
            set => SetValue(XamlProperty, value);
        }

        public static readonly AvaloniaProperty<ISourceFile> SourceFileProperty =
            AvaloniaProperty.Register<AvaloniaPreviewer, ISourceFile>(nameof(SourceFile), defaultBindingMode: BindingMode.TwoWay);

        public ISourceFile SourceFile
        {
            get => GetValue(SourceFileProperty);
            set => SetValue(SourceFileProperty, value);
        }

        private void OnSourceFileChanged(ISourceFile file)
        {
            KillHost();

            if (file != null)
            {
                StartPreviewerProcess(file);
            }
        }

        private void StartPreviewerProcess (ISourceFile file)
        {
            if (File.Exists(file.Project.Executable))
            {
                var port = FreeTcpPort();

                new BsonTcpTransport().Listen(IPAddress.Loopback, port, t =>
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        _connection = t;

                        _remoteContainer.Child = _remote = new RemoteWidget(t);

                        t.Send(new UpdateXamlMessage
                        {
                            Xaml = Xaml
                        });

                        t.OnMessage += OnMessage;
                    });

                });

                var executingDir = Platforms.Platform.ExecutionPath;

                var projectVariables = file.Project.GetEnvironmentVariables();


                var projectDir = Path.GetDirectoryName(file.Project.Executable);

                var args = $@"exec --runtimeconfig $(TargetDir)$(TargetName).runtimeconfig.json --depsfile $(TargetDir)$(TargetName).deps.json {executingDir}\HostApp\Avalonia.Designer.HostApp.dll --transport tcp-bson://127.0.0.1:{port}/ $(TargetPath)".ExpandVariables(projectVariables);
                _currentHost = Process.Start("dotnet", args);
            }
        }

        private void KillHost()
        {
            if(_connection != null)
            {
                _connection.Dispose();
                _connection.OnMessage -= OnMessage;
            }

            if (_currentHost != null)
            {
                if (!_currentHost.HasExited)
                {
                    _currentHost?.Kill();
                }

                _currentHost = null;
            }

            if (_remote != null)
            {
                _remote.IsVisible = false;
            }
        }

        public AvaloniaPreviewer()
        {
            this.GetObservable(XamlProperty).Subscribe(xaml =>
            {
                _connection?.Send(new UpdateXamlMessage
                {
                    Xaml = Xaml
                });
            });

            this.GetObservable(SourceFileProperty).Subscribe(file =>
            {
                OnSourceFileChanged(file);
            });

            var shell = IoC.Get<IShell>();

            shell.BuildStarting += (sender, e) =>
            {
                if (SourceFile.Project == e.Project && _currentHost != null)
                {
                    KillHost();
                }
            };

            shell.BuildCompleted += (sender, e) =>
            {
                if (SourceFile != null  && SourceFile.Project == e.Project)
                {
                    StartPreviewerProcess(SourceFile);
                }
            };
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            KillHost();
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            var scrollViewer = e.NameScope.Find<ScrollViewer>("PART_Remote");

            _remoteContainer = new Center();

            scrollViewer.Content = _remoteContainer;
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
    }
}
