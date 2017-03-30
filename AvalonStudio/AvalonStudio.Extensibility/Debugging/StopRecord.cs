using System;

namespace AvalonStudio.Debugging
{
	public enum StopReason
	{
        EntryPointHit,
		BreakPointHit,
		WatchPointTrigger,
		ReadWatchPointTrigger,
		AccessWatchPointTrigger,
		FunctionFinished,
		LocationReached,
		WatchPointScope,
		EndSteppingRange,
		ExitedSignalled,
		Exited,
		ExitedNormally,
		SignalReceived,
		SolibEvent,
		Fork,
		VFork,
		SyscallEntry,
		Exec
	}

	public class StopRecord
	{
		public StopReason Reason { get; set; }
		public Frame Frame { get; set; }
		public uint ThreadId { get; set; }
		public uint BreakPointNumber { get; set; }
		public bool KeepBreakPoint { get; set; }
		public string StoppedThreads { get; set; }
	}
}