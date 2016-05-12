namespace AvalonStudio.Debugging
{
    using Extensibility.Plugin;
    using Avalonia.Controls;
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Toolchains;
    using Utils;

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

    [InheritedExport(typeof(IDebugger))]
    public interface IDebugger
    {
        UserControl GetSettingsControl(IProject project);

        void ProvisionSettings(IProject project);

        DebuggerState State { get; }

        bool DebugMode { get; set; }

        /// <summary>
        /// Gets a unique id that can be used to reference a variable or expression.
        /// The id is unique to the debug session.
        /// </summary>
        /// <returns>the id</returns>
        int GetVariableId();

        event EventHandler<StopRecord> Stopped;
        event EventHandler<EventArgs> StateChanged;

        void Initialise();

        List<VariableObjectChange> UpdateVariables();

        void Run();

        Dictionary<int, Register> GetRegisters();

        Dictionary<int, string> GetChangedRegisters();

        List<InstructionLine> Disassemble(string file, int line, int numLines);

        List<InstructionLine> Disassemble(ulong start, uint count);

        List<MemoryBytes> ReadMemoryBytes(ulong address, ulong offset, uint count);

        List<Variable> ListStackVariables();

        List<Frame> ListStackFrames();

        void StepOver();

        void StepOut();

        void StepInto();

        void StepInstruction();

        /// <summary>
        /// Pause the debugger/
        /// </summary>
        /// <returns>true if the pause request generated the pause, false if the debugger stopped before the request could be completed.</returns>
        bool Pause();

        void Continue();

        string EvaluateExpression(string expression);

        List<VariableObject> ListChildren(VariableObject variable);

        VariableObject CreateWatch(string id, string expression);
        void DeleteWatch(string id);

        void SetWatchFormat(string id, WatchFormat format);

        void Stop();

        void Close();

        bool Start(IToolChain toolChain, IConsole console, IProject project);

        void Reset(bool runAfter);

        LiveBreakPoint BreakMain();

        LiveBreakPoint SetBreakPoint(string file, UInt32 line);

        void Remove(LiveBreakPoint breakPoint);
    }
}
