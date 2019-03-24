using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Remote;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Remote.Protocol;
using Avalonia.Remote.Protocol.Designer;
using Avalonia.Remote.Protocol.Viewport;
using Avalonia.Threading;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Disposables;
using System.Reactive.Linq;

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
        private Grid _overlay;
        private Grid _errorOverlay;
        private TextBlock _statusText;
        private TextBox _errorText;
        private CompositeDisposable _disposables;
        private IDisposable _listener;
        private VisualBrush _visualBrush;
        private CheckBox _showErrors;

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

        private void StartPreviewerProcess(ISourceFile file)
        {
            if (File.Exists(file.Project.Executable))
            {
                var port = FreeTcpPort();

                _listener = new BsonTcpTransport().Listen(IPAddress.Loopback, port, t =>
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        _connection = t;

                        _remoteContainer.Child = _remote = new RemoteWidget(t);

                        t.Send(new UpdateXamlMessage
                        {
                            Xaml = Xaml,
                            AssemblyPath = SourceFile.Project.Executable
                        });

                        t.OnMessage += OnMessage;
                    });

                });

                var executingDir = Platforms.Platform.ExecutionPath;

                var projectVariables = file.Project.Solution.StartupProject.GetEnvironmentVariables();

                var projectDir = System.IO.Path.GetDirectoryName(file.Project.Solution.StartupProject.Executable);

                var args = $@"exec --runtimeconfig $(TargetDir)$(TargetName).runtimeconfig.json --depsfile $(TargetDir)$(TargetName).deps.json {executingDir}/Avalonia.Designer.HostApp.dll --transport tcp-bson://127.0.0.1:{port}/ $(TargetPath)".ExpandVariables(projectVariables);

                if (_overlay != null)
                {
                    _overlay.IsVisible = false;
                    _statusText.Text = "";
                }

                _currentHost = PlatformSupport.LaunchShellCommand("dotnet", args, (sender, e) =>
                {
                },
                (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            if (!_overlay.IsVisible)
                            {
                                _statusText.Text = "Your app must target Avalonia version >= '0.7.0' to be compatible with the previewer.\r\n\r\n";
                            }

                            _statusText.Text += e.Data + "\r\n";

                            _overlay.IsVisible = true;
                        });
                    }
                }, false, executeInShell: false);
            }
            else
            {
                Dispatcher.UIThread.Post(() =>
                {
                    _statusText.Text = "Please build your project to enable previewing and intellisense.";
                    _overlay.IsVisible = true;
                });
            }
        }

        private void KillHost()
        {
            _listener?.Dispose();

            if (_connection != null)
            {
                _connection.OnMessage -= OnMessage;
                _connection.Dispose();
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
            AddHandler(PointerWheelChangedEvent, (sender, e) =>
            {
                if (e.InputModifiers.HasFlag(InputModifiers.Control) && _remote != null)
                {
                    var delta = e.Delta.Y / 25;

                    if (e.InputModifiers.HasFlag(InputModifiers.Shift))
                    {
                        delta = e.Delta.Y / 100;
                    }

                    var zoomLevel = _remote.ZoomLevel + delta;

                    if (zoomLevel < 0.25)
                    {
                        zoomLevel = 0.25;
                    }

                    _remote.ZoomLevel = zoomLevel;

                    e.Handled = true;
                }
            }, Avalonia.Interactivity.RoutingStrategies.Tunnel);

            _visualBrush = new VisualBrush
            {
                DestinationRect = new RelativeRect(0, 0, 20, 20, RelativeUnit.Absolute),
                TileMode = TileMode.Tile,
                Visual = new Canvas
                {
                    Width = 20,
                    Height = 20,
                    Background = ColorScheme.CurrentColorScheme.Background,
                    Children =
                    {
                        new Rectangle
                        {
                            Height = 10,
                            Width = 10,
                            Fill = ColorScheme.CurrentColorScheme.BackgroundAccent
                        },
                        new Rectangle
                        {
                            Height = 10,
                            Width = 10,
                            Fill = ColorScheme.CurrentColorScheme.BackgroundAccent,
                            [Canvas.LeftProperty] = 10,
                            [Canvas.TopProperty] = 10
                        }
                    }
                }
            };

            var studio = IoC.Get<IStudio>();

            _disposables = new CompositeDisposable
            {
                this.GetObservable(XamlProperty)
                .Throttle(TimeSpan.FromMilliseconds(100))
                .ObserveOn(AvaloniaScheduler.Instance)
                .Subscribe(xaml =>
                {
                    _connection?.Send(new UpdateXamlMessage
                    {
                        Xaml = Xaml
                    });
                }),

                this.GetObservable(SourceFileProperty).Subscribe(file =>
                {
                    OnSourceFileChanged(file);
                }),

                Observable.FromEventPattern<BuildEventArgs>(studio, nameof(studio.BuildStarting)).Subscribe(o =>
                {
                    if (SourceFile.Project.Solution.StartupProject == o.EventArgs.Project && _currentHost != null)
                    {
                        KillHost();
                    }
                }),

                Observable.FromEventPattern<BuildEventArgs>(studio, nameof(studio.BuildCompleted)).Subscribe(o =>
                {
                    if (SourceFile != null && SourceFile.Project.Solution.StartupProject == o.EventArgs.Project)
                    {
                        StartPreviewerProcess(SourceFile);
                    }
                })
            };
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            KillHost();
            _disposables.Dispose();
        }

        protected override void OnTemplateApplied(TemplateAppliedEventArgs e)
        {
            base.OnTemplateApplied(e);

            _remoteContainer = e.NameScope.Find<Center>("PART_Center");

            _overlay = e.NameScope.Find<Grid>("PART_Overlay");
            _statusText = e.NameScope.Find<TextBlock>("PART_Status");

            _errorOverlay = e.NameScope.Find<Grid>("PART_ErrorOverlay");
            _errorText = e.NameScope.Find<TextBox>("PART_Errors");

            _showErrors = e.NameScope.Find<CheckBox>("PART_ShowErrors");

            var background = e.NameScope.Find<ScrollViewer>("PART_Remote");

            background.Background = _visualBrush;
        }

        private void OnMessage(IAvaloniaRemoteTransportConnection transport, object obj)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                if (transport != _connection)
                    return;
                if (obj is UpdateXamlResultMessage result)
                {
                    if (result.Error != null)
                    {
                        _errorText.Text = result.Error;
                        _showErrors.IsVisible = true;

                        _remote.InError = true;
                    }
                    else
                    {
                        _remote.InError = false;
                        _showErrors.IsVisible = false;
                        _errorText.Text = "";
                    }
                }
            });
        }
    }
}
