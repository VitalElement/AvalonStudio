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
    using System.Xml;
    using System;

    public class DebugManager2 : IDebugManager2, IExtension
    {
        private DebuggerSession _session;
        private IShell _shell;
        private IConsole _console;
        private IEditor _lastDocument;

        public event EventHandler DebugSessionStarted;
        public event EventHandler DebugSessionEnded;
        public event EventHandler<TargetEventArgs> TargetStopped;

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
            DebugSessionEnded?.Invoke(this, new EventArgs());

            _shell.CurrentPerspective = Perspective.Editor;

            if (_session != null)
            {
                _session.TargetStopped -= _session_TargetStopped;
                _session.TargetHitBreakpoint -= _session_TargetStopped;
                _session.TargetExited -= _session_TargetExited;
                _session.TargetStarted -= _session_TargetStarted;
            }

            _session?.Dispose();
            _session = null;

            _lastDocument?.ClearDebugHighlight();
            _lastDocument = null;

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

            if (!await project.ToolChain.Build(_console, project))
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

            _session = project.Debugger2.CreateSession(project);

            _session.Breakpoints = Breakpoints;

            _session.Run(project.Debugger2.GetDebuggerStartInfo(project), project.Debugger2.GetDebuggerSessionOptions(project));

            _session.TargetStopped += _session_TargetStopped;

            _session.TargetHitBreakpoint += _session_TargetStopped;

            _session.TargetSignaled += _session_TargetStopped;

            _session.TargetInterrupted += _session_TargetStopped;

            _session.TargetExited += _session_TargetExited;

            _session.TargetStarted += _session_TargetStarted;

            _shell.CurrentPerspective = Perspective.Debug;

            DebugSessionStarted?.Invoke(this, new EventArgs());
        }

        private void _session_TargetStarted(object sender, EventArgs e)
        {
            if (_lastDocument != null)
            {
                _lastDocument.ClearDebugHighlight();
                _lastDocument = null;
            }
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
                    var normalizedPath = sourceLocation.FileName.Replace("\\\\", "\\").NormalizePath();

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
                        Dispatcher.UIThread.InvokeAsync(async () => { _lastDocument = await _shell.OpenDocument(file, sourceLocation.Line, sourceLocation.Column, sourceLocation.EndColumn, true); });
                    }
                    else
                    {
                        _console.WriteLine("Unable to find file: " + normalizedPath);
                    }
                }

                TargetStopped?.Invoke(this, e);
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
