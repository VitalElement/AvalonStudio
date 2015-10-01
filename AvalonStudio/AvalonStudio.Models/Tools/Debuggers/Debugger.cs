using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Models.Solutions;
using AvalonStudio.Models.Tools.Compiler;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public enum DebuggerState
    {
        NotRunning,
        Running,
        Paused
    }

    public enum WatchFormat
    {
        Binary,
        Decimal,
        Hexadecimal,
        Octal,
        Natural        
    }

    public abstract class Debugger
    {
        public abstract DebuggerState State { get; }

        public bool DebugMode { get; set; }

        public abstract event EventHandler<StopRecord> Stopped;
        public abstract event EventHandler<EventArgs> StateChanged;

        public abstract void Initialise ();

        public abstract List<VariableObjectChange> UpdateVariables();

        public abstract void Run ();

        public abstract Dictionary<int, Register> GetRegisters();

        public abstract Dictionary<int, string> GetChangedRegisters();

        public abstract List<InstructionLine> Disassemble (string file, int line, int numLines);

        public abstract List<InstructionLine> Disassemble (ulong start, uint count);

        public abstract List<MemoryBytes> ReadMemoryBytes(ulong address, ulong offset, uint count);

        public abstract List<Variable> ListStackVariables ();

        public abstract List<Frame> ListStackFrames();

        public abstract void StepOver ();

        public abstract void StepOut ();

        public abstract void StepInto ();

        public abstract void StepInstruction ();

        /// <summary>
        /// Pause the debugger/
        /// </summary>
        /// <returns>true if the pause request generated the pause, false if the debugger stopped before the request could be completed.</returns>
        public abstract bool Pause ();

        public abstract void Continue ();

        public abstract VariableObject CreateWatch (string id, string expression);

        public abstract void SetWatchFormat(string id, WatchFormat format);  

        public abstract void Stop ();

        public abstract void Close ();

        public abstract bool Start (ToolChain toolChain, IConsole console, Project project);

        public abstract void Reset (bool runAfter);

        public abstract LiveBreakPoint BreakMain ();

        public abstract LiveBreakPoint SetBreakPoint (string file, UInt32 line);

        public abstract void Remove (LiveBreakPoint breakPoint);
    }
}
