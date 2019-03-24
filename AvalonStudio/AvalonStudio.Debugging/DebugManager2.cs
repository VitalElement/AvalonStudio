namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Shell;
    using AvalonStudio.Extensibility.Studio;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using AvalonStudio.Utils;
    using Mono.Debugging.Client;
    using System;
    using System.Composition;
    using System.Reactive.Linq;
    using System.Xml;

    [Export(typeof(IExtension)), Export(typeof(IDebugManager2)), Shared]
    public class DebugManager2 : IDebugManager2, IActivatableExtension
    {
        private DebuggerSession _session;
        private object _sessionLock = new object();
        private StackFrame _currentStackFrame;

        private IStudio _studio;        
        private IConsole _console;
        private IDebugLineDocumentTabViewModel _lastDocument;

        public event EventHandler DebugSessionStarted;

        public event EventHandler DebugSessionEnded;

        public event EventHandler<TargetEventArgs> TargetReady;

        public event EventHandler<TargetEventArgs> TargetStopped;

        public event EventHandler<EventArgs> TargetStarted;

        public event EventHandler FrameChanged;

        public DebugManager2()
        {
            Breakpoints = new BreakpointStore();

            Breakpoints.BreakpointAdded += (sender, e) =>
            {
                SaveBreakpoints();
            };

            Breakpoints.BreakpointRemoved += (sender, e) =>
            {
                SaveBreakpoints();
            };
        }

        public void SetFrame(StackFrame frame)
        {
            _currentStackFrame = frame;

            FrameChanged?.Invoke(this, EventArgs.Empty);
        }

        public StackFrame SelectedFrame
        {
            get
            {
                return _currentStackFrame;
            }
            set
            {
                SetFrame(value);
            }
        }

        private bool _loadingBreakpoints;

        private void SaveBreakpoints()
        {
            if (!_loadingBreakpoints)
            {
                var solution = _studio.CurrentSolution;

                Platform.EnsureSolutionUserDataDirectory(solution);

                var file = System.IO.Path.Combine(Platform.GetUserDataDirectory(solution), "Breakpoints.xml");

                using (var writer = XmlWriter.Create(file))
                {
                    Breakpoints.Save().WriteTo(writer);
                }
            }
        }

        private void LoadBreakpoints()
        {
            _loadingBreakpoints = true;

            var solution = _studio.CurrentSolution;

            if (solution != null)
            {
                var file = System.IO.Path.Combine(Platform.GetUserDataDirectory(solution), "Breakpoints.xml");

                if (System.IO.File.Exists(file))
                {
                    using (var reader = XmlReader.Create(file))
                    {
                        var doc = new XmlDocument();
                        doc.Load(reader);

                        Breakpoints.Load(doc.DocumentElement);
                    }
                }
            }
            else
            {
                Breakpoints.Clear();
            }

            _loadingBreakpoints = false;
        }

        public DebuggerSession Session => _session;

        public ExtendedDebuggerSession ExtendedSession => _session as ExtendedDebuggerSession;

        public BreakpointStore Breakpoints { get; set; }

        public bool SessionActive => _session != null;

        public IObservable<bool> CanStart { get; private set; }

        public IObservable<bool> CanPause { get; private set; }

        public IObservable<bool> CanStop { get; private set; }

        public IObservable<bool> CanStep { get; private set; }

        public void Activation()
        {
            _studio = IoC.Get<IStudio>();
            _console = IoC.Get<IConsole>();

            var started = Observable.FromEventPattern(this, nameof(TargetStarted)).Select(e => true);
            var stopped = Observable.FromEventPattern(this, nameof(TargetStopped)).Select(e => false);
            var sessionStarted = Observable.FromEventPattern(this, nameof(DebugSessionStarted)).Select(e => true);
            var sessionEnded = Observable.FromEventPattern(this, nameof(DebugSessionEnded)).Select(e => false);

            var hasSession = sessionStarted.Merge(sessionEnded).StartWith(false);

            var isRunning = hasSession.Merge(started).Merge(stopped).StartWith(false);

            var canRun = _studio.OnSolutionLoaded().CombineLatest(isRunning, hasSession, _studio.OnCurrentTaskChanged(), (loaded, running, session, hasTask) =>
            {
                return loaded && !running && (!hasTask || (hasTask && session));
            });

            var canPause = _studio.OnSolutionLoaded().CombineLatest(isRunning, (loaded, running) => loaded && running);

            var canStop = _studio.OnSolutionLoaded().CombineLatest(sessionStarted.Merge(sessionEnded), (loaded, sessionActive) => loaded && SessionActive);

            var canStep = canStop.CombineLatest(isRunning, (stop, running) => stop && !running);

            CanStart = canRun.StartWith(false);

            CanPause = canPause.StartWith(false);

            CanStop = canStop.StartWith(false);

            CanStep = canStep.StartWith(false);

            _studio.OnSolutionChanged.Subscribe(_ => LoadBreakpoints());
        }

        public void BeforeActivation()
        {
        }

        private void OnEndSession()
        {
            lock (_sessionLock)
            {
                if (_session != null)
                { 
                    _session.TargetUnhandledException -= _session_TargetStopped;
                    _session.TargetStopped -= _session_TargetStopped;
                    _session.TargetHitBreakpoint -= _session_TargetStopped;
                    _session.TargetSignaled -= _session_TargetStopped;
                    _session.TargetInterrupted -= _session_TargetStopped;
                    _session.TargetExited -= _session_TargetExited;
                    _session.TargetStarted -= _session_TargetStarted;
                    _session.TargetReady -= _session_TargetReady;

                    _session?.Dispose();
                    _session = null;
                }
            }

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DebugSessionEnded?.Invoke(this, EventArgs.Empty);

                _studio.CurrentPerspective = Perspective.Normal;

                _lastDocument?.ClearDebugHighlight();
                _lastDocument = null;

                // This will save breakpoints that were moved to be closer to actual sequence points.
                Breakpoints.Save();
            });
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            _session?.Exit();
        }

        public async void Start()
        {
            var project = _studio.GetDefaultProject();

            if (project == null)
            {
                OnEndSession();
                _console.WriteLine("No Default project set. Please set a default project before debugging.");
                return;
            }

            bool success = false;

            success = await _studio.BuildAsync(project);

            if (!success)
            {
                OnEndSession();
                return;
            }

            if (project.Debugger2 == null)
            {
                OnEndSession();
                _console.WriteLine("No Debug adaptor is set for default project.");
                return;
            }

            var debugger2 = project.Debugger2 as IDebugger2;

            if (await debugger2.InstallAsync(IoC.Get<IConsole>(), project))
            {
                _session = debugger2.CreateSession(project);

                _session.TargetUnhandledException += _session_TargetStopped;
                _session.TargetStopped += _session_TargetStopped;
                _session.TargetHitBreakpoint += _session_TargetStopped;
                _session.TargetSignaled += _session_TargetStopped;
                _session.TargetInterrupted += _session_TargetStopped;
                _session.TargetExited += _session_TargetExited;
                _session.TargetStarted += _session_TargetStarted;
                _session.TargetReady += _session_TargetReady;

                _session.Breakpoints = Breakpoints;

                _session.Run(debugger2.GetDebuggerStartInfo(project), debugger2.GetDebuggerSessionOptions(project));

                _studio.CurrentPerspective = Perspective.Debugging;

                DebugSessionStarted?.Invoke(this, EventArgs.Empty);
            }
        }

        private void _session_TargetReady(object sender, TargetEventArgs e)
        {
            TargetReady?.Invoke(this, e);
        }

        private void _session_TargetStarted(object sender, EventArgs e)
        {
            if (_lastDocument != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _lastDocument.ClearDebugHighlight();
                    _lastDocument = null;
                });
            }

            TargetStarted?.Invoke(this, e);
        }

        private void _session_TargetExited(object sender, TargetEventArgs e)
        {
            OnEndSession();
        }

        private void _session_TargetStopped(object sender, TargetEventArgs e)
        {
            if (e.Backtrace != null && e.Backtrace.FrameCount > 0)
            {
                var currentFrame = e.Backtrace.GetFrame(0);

                var sourceLocation = currentFrame.SourceLocation;

                if (sourceLocation.FileName != null)
                {
                    var normalizedPath = sourceLocation.FileName.NormalizePath();

                    ISourceFile file = null;

                    var document = _studio.GetEditor(normalizedPath);

                    if (document != null)
                    {
                        _lastDocument = document as IDebugLineDocumentTabViewModel;
                        file = document?.SourceFile;
                    }

                    if (file == null)
                    {
                        file = _studio.CurrentSolution.FindFile(normalizedPath);
                    }

                    if (file != null)
                    {
                        Dispatcher.UIThread.InvokeAsync(async () =>
                        {
                            _lastDocument = await _studio.OpenDocumentAsync(file, sourceLocation.Line, sourceLocation.Column, sourceLocation.EndColumn, true)
                                                as IDebugLineDocumentTabViewModel;
                        }).Wait();
                    }
                    else
                    {
                        _console.WriteLine("Unable to find file: " + normalizedPath);
                    }
                }

                if (e.BreakEvent is WatchPoint)
                {
                    var wp = e.BreakEvent as WatchPoint;

                    _console.WriteLine($"Hit Watch Point {wp.Expression}");
                }

                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    TargetStopped?.Invoke(this, e);
                    SetFrame(currentFrame);
                }).Wait();
            }
        }

        public void StepOver()
        {
            _session?.NextLine();
        }

        public void Continue()
        {
            _session?.Continue();
        }

        public void StepInto()
        {
            _session?.StepLine();
        }

        public void StepInstruction()
        {
            _session?.StepInstruction();
        }

        public void StepOut()
        {
            _session?.Finish();
        }

        public void Pause()
        {
            _session?.Stop();
        }
    }
}