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
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;

    public interface IDebugManager2
    {
        IDebugger2 CurrentDebugger { get; set; }

        BreakpointStore Breakpoints { get; set; }

        void Start();

        void Continue();

        void SteoOver();

        void StepInto();

        void StepInstruction();

        void StepOut();
    }

    public class DebugManager2 : IDebugManager2, IExtension
    {
        private DebuggerSession _session;
        private IShell _shell;
        private IEditor _lastDocument;

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

        public BreakpointStore Breakpoints { get; set; }

        public IDebugger2 CurrentDebugger { get; set; }

        public void Activation()
        {
            _shell = IoC.Get<IShell>();

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
            CurrentDebugger = null;
            _session?.Dispose();
        }

        public async void Start()
        {
            if (CurrentDebugger != null)
            {
                var project = _shell.GetDefaultProject();

                if(!await project.ToolChain.Build(IoC.Get<IConsole>(), project))
                {
                    return;
                }
                
                _session = CurrentDebugger.CreateSession();

                _session.Breakpoints = Breakpoints;

                _session.Run(CurrentDebugger.GetDebuggerStartInfo(project), CurrentDebugger.GetDebuggerSessionOptions(project));

                _session.TargetStopped += _session_TargetStopped;

                _session.TargetEvent += (sender, e) =>
                {
                    IoC.Get<IConsole>().WriteLine(e.Type.ToString());
                };

                _session.TargetHitBreakpoint += _session_TargetStopped;

                _session.TargetExited += (sender, e) =>
                {
                    OnEndSession();

                    if (_lastDocument != null)
                    {
                        _lastDocument.ClearDebugHighlight();
                        _lastDocument = null;
                    }
                };

                _session.TargetStarted += (sender, e) =>
                {
                    if (_lastDocument != null)
                    {
                        _lastDocument.ClearDebugHighlight();
                        _lastDocument = null;
                    }
                };

                _session.OutputWriter = (stdError, text) => 
                {
                    IoC.Get<IConsole>().Write(text);
                };
            }
        }

        private void _session_TargetStopped(object sender, TargetEventArgs e)
        {            
            var sourceLocation = e.Backtrace.GetFrame(0).SourceLocation;

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
                Dispatcher.UIThread.InvokeAsync(async () => { _lastDocument = await _shell.OpenDocument(file, sourceLocation.Line, 1, true); });
            }
            else
            {
                IoC.Get<IConsole>().WriteLine("Unable to find file: " + normalizedPath);
            }
        }

        public void SteoOver()
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
    }
}
