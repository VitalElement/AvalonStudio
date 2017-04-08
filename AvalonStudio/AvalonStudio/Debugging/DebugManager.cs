namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
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

    public interface IDebugManager2
    {
        IDebugger2 CurrentDebugger { get; set; }

        void Start();

        void StepLine();
    }

    public class DebugManager2 : IDebugManager2, IExtension
    {
        private DebuggerSession _session;

        public DebugManager2()
        {

        }

        public IDebugger2 CurrentDebugger { get; set; }

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant<IDebugManager2>(this);
        }

        public void Start()
        {
            if(CurrentDebugger != null)
            {
                var project = IoC.Get<IShell>().GetDefaultProject();

                _session = CurrentDebugger.CreateSession();

                _session.Breakpoints.Add("c:\\dev\\repos\\dotnettest\\Program.cs", 9);

                _session.Run(CurrentDebugger.GetDebuggerStartInfo(project), CurrentDebugger.GetDebuggerSessionOptions(project));

                _session.TargetStopped += _session_TargetStopped;

                _session.TargetEvent += (sender, e) =>
                {
                    IoC.Get<IConsole>().WriteLine(e.Type.ToString());
                };

                _session.TargetHitBreakpoint += _session_TargetStopped;

                _session.TargetExited += (sender, e) =>
                {
                    _session.Dispose();
                    //_session.Exit();
                };
            }
        }

        private void _session_TargetStopped(object sender, TargetEventArgs e)
        {
            var _shell = IoC.Get<IShell>();

            var sourceLocation = e.Backtrace.GetFrame(0).SourceLocation;

            var normalizedPath = sourceLocation.FileName.Replace("\\\\", "\\").NormalizePath();

            ISourceFile file = null;

            var document = _shell.GetDocument(normalizedPath);

            if (document != null)
            {
                //lastDocument = document;
                file = document?.ProjectFile;
            }

            if (file == null)
            {
                file = _shell.CurrentSolution.FindFile(normalizedPath);
            }

            if (file != null)
            {
                Dispatcher.UIThread.InvokeAsync(async () => { await _shell.OpenDocument(file, sourceLocation.Line, 1, true); });
            }
            else
            {
                IoC.Get<IConsole>().WriteLine("Unable to find file: " + normalizedPath);
            }
        }

        public void StepLine()
        {
            _session?.StepLine();
        }
    }
}
