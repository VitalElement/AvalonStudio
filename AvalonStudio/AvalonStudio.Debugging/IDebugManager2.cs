namespace AvalonStudio.Debugging
{
    using Mono.Debugging.Client;
    using System;

    public interface IDebugManager2
    {
        event EventHandler DebugSessionStarted;

        event EventHandler DebugSessionEnded;

        event EventHandler FrameChanged;

        event EventHandler<TargetEventArgs> TargetReady;

        event EventHandler<TargetEventArgs> TargetStopped;

        event EventHandler<EventArgs> TargetStarted;

        StackFrame SelectedFrame { get; set; }

        DebuggerSession Session { get; }

        ExtendedDebuggerSession ExtendedSession { get; }

        BreakpointStore Breakpoints { get; set; }

        void Start();

        void Restart();

        void Stop();

        void Continue();

        void Pause();

        void StepOver();

        void StepInto();

        void StepInstruction();

        void StepOut();

        bool SessionActive { get; }
    }
}