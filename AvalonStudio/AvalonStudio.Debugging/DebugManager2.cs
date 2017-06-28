namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Plugin;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Shell;
    using AvalonStudio.Utils;
    using Mono.Debugging.Client;
    using System;
    using System.Threading.Tasks;
    using System.Xml;

    public class DebugManager2 : IDebugManager2, IExtension
    {
        private DebuggerSession _session;
        private StackFrame _currentStackFrame;

        private IShell _shell;
        private IConsole _console;
        private IEditor _lastDocument;

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

            FrameChanged?.Invoke(this, new EventArgs());
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
                var solution = _shell.CurrentSolution;

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

            var solution = _shell.CurrentSolution;

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

        public void Activation()
        {
            _shell = IoC.Get<IShell>();
            _console = IoC.Get<IConsole>();

            _shell.SolutionChanged += (sender, e) =>
            {
                LoadBreakpoints();
            };
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<IDebugManager2>(this);
        }

        private void OnEndSession()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DebugSessionEnded?.Invoke(this, new EventArgs());

                _shell.CurrentPerspective = Perspective.Editor;

                _lastDocument?.ClearDebugHighlight();
                _lastDocument = null;
            });

            if (_session != null)
            {
                _session.Exit();
                _session.TargetStopped -= _session_TargetStopped;
                _session.TargetHitBreakpoint -= _session_TargetStopped;
                _session.TargetExited -= _session_TargetExited;
                _session.TargetStarted -= _session_TargetStarted;
                _session.TargetReady -= _session_TargetReady;
                _session.Dispose();
                _session = null;
            }

            // This will save breakpoints that were moved to be closer to actual sequence points.
            Breakpoints.Save();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            OnEndSession();
        }

        public async void Start()
        {
            var project = _shell.GetDefaultProject();

            if (project == null)
            {
                OnEndSession();
                _console.WriteLine("No Default project set. Please set a default project before debugging.");
                return;
            }

            var success = await await Task.Factory.StartNew(async () => { return await project.ToolChain.Build(_console, project); });

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

            await debugger2.InstallAsync(IoC.Get<IConsole>());

            _session = debugger2.CreateSession(project);

            _session.Breakpoints = Breakpoints;

            _session.Run(debugger2.GetDebuggerStartInfo(project), debugger2.GetDebuggerSessionOptions(project));

            _session.TargetStopped += _session_TargetStopped;

            _session.TargetHitBreakpoint += _session_TargetStopped;

            _session.TargetSignaled += _session_TargetStopped;

            _session.TargetInterrupted += _session_TargetStopped;

            _session.TargetExited += _session_TargetExited;

            _session.TargetStarted += _session_TargetStarted;

            _session.TargetReady += _session_TargetReady;

            _shell.CurrentPerspective = Perspective.Debug;

            DebugSessionStarted?.Invoke(this, new EventArgs());
        }

        private void _session_TargetReady(object sender, TargetEventArgs e)
        {
            TargetReady?.Invoke(this, e);
        }

        private void _session_TargetStarted(object sender, EventArgs e)
        {
            if (_lastDocument != null)
            {
                _lastDocument.ClearDebugHighlight();
                _lastDocument = null;
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

                    var document = _shell.GetDocument(normalizedPath);

                    if (document != null)
                    {
                        _lastDocument = document;
                        file = document?.ProjectFile;
                    }

                    if (file == null)
                    {
                        file = _shell.CurrentSolution.FindFile(normalizedPath);
                    }

                    if (file != null)
                    {
                        Dispatcher.UIThread.InvokeTaskAsync(async () => { _lastDocument = await _shell.OpenDocument(file, sourceLocation.Line, sourceLocation.Column, sourceLocation.EndColumn, true); }).Wait();
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

                Dispatcher.UIThread.InvokeTaskAsync(() =>
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