namespace AvalonStudio.Debugging
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FrameChangedEventArgs : EventArgs
    {
        public List<VariableObjectChange> VariableChanges;
        public ulong Address;
    }

    public interface IDebugManager
    {
        /// <summary>
        /// The project currently being debugged.
        /// </summary>
        IProject Project { get; }

        IDebugger CurrentDebugger { get; }

        event EventHandler<FrameChangedEventArgs> DebugFrameChanged;
        event EventHandler DebuggerChanged;
        event EventHandler DebugSessionStarted;
        event EventHandler DebugSessionEnded;

        BreakPointManager BreakPointManager { get; }

        void StartDebug(IProject project);

        void Restart();

        void Continue();

        void StepOver();

        void StepInstruction();

        void StepInto();

        void StepOut();

        void Stop();

        void Pause();
    }
}
