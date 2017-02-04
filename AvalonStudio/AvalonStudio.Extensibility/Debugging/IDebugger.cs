using System;
using System.Collections.Generic;
using System.Composition;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Debugging
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
    
	public interface IDebugger : IExtension
	{
		DebuggerState State { get; }

		bool DebugMode { get; set; }
		object GetSettingsControl(IProject project);

		void ProvisionSettings(IProject project);

		/// <summary>
		///     Gets a unique id that can be used to reference a variable or expression.
		///     The id is unique to the debug session.
		/// </summary>
		/// <returns>the id</returns>
		int GetVariableId();

		event EventHandler<StopRecord> Stopped;
		event EventHandler<EventArgs> StateChanged;

		void Initialise();

		Task<List<VariableObjectChange>> UpdateVariablesAsync();

		Task RunAsync();

		Task<Dictionary<int, Register>> GetRegistersAsync();

		Task<Dictionary<int, string>> GetChangedRegistersAsync();

		Task<List<InstructionLine>> DisassembleAsync(string file, int line, int numLines);

		Task<List<InstructionLine>> DisassembleAsync(ulong start, uint count);

		Task<List<MemoryBytes>> ReadMemoryBytesAsync(ulong address, ulong offset, uint count);

		Task<List<Variable>> ListStackVariablesAsync();

		Task<List<Frame>> ListStackFramesAsync();

		Task StepOverAsync();

		Task StepOutAsync();

		Task StepIntoAsync();

		Task StepInstructionAsync();

		/// <summary>
		///     Pause the debugger/
		/// </summary>
		/// <returns>
		///     true if the pause request generated the pause, false if the debugger stopped before the request could be
		///     completed.
		/// </returns>
		Task<bool> PauseAsync();

		Task ContinueAsync();

		Task<string> EvaluateExpressionAsync(string expression);

		Task<List<VariableObject>> ListChildrenAsync(VariableObject variable);

		Task<VariableObject> CreateWatchAsync(string id, string expression);

		Task DeleteWatchAsync(string id);

		Task SetWatchFormatAsync(string id, WatchFormat format);

		Task StopAsync();

		Task CloseAsync();

		Task<bool> StartAsync(IToolChain toolChain, IConsole console, IProject project);

		Task ResetAsync(bool runAfter);

		Task<LiveBreakPoint> BreakMainAsync();

		Task<LiveBreakPoint> SetBreakPointAsync(string file, uint line);

		Task RemoveAsync(LiveBreakPoint breakPoint);
	}
}