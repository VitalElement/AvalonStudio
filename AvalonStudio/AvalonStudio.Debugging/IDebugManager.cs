using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AvalonStudio.Projects;

namespace AvalonStudio.Debugging
{
	public class FrameChangedEventArgs : EventArgs
	{
		public ulong Address;
		public List<VariableObjectChange> VariableChanges;
	}

	public interface IDebugManager
	{
		/// <summary>
		///     The project currently being debugged.
		/// </summary>
		IProject Project { get; }

		IDebugger CurrentDebugger { get; }

		BreakPointManager BreakPointManager { get; }

		event EventHandler<FrameChangedEventArgs> DebugFrameChanged;
		event EventHandler DebuggerChanged;
		event EventHandler DebugSessionStarted;
		event EventHandler DebugSessionEnded;

		Task<VariableObject> ProbeExpressionAsync(string expression);

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