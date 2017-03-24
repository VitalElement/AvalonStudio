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

		public static StopRecord FromArgumentList(NameValuePair[] arguments)
		{
			var result = new StopRecord();

			foreach (var arg in arguments)
			{
				switch (arg.Name)
				{
					case "reason":
						result.Reason = arg.Value.ToStopReason();
						break;

					case "frame":
						result.Frame = Frame.FromDataString(arg.Value);
						break;

					case "thread-id":
						result.ThreadId = Convert.ToUInt32(arg.Value);
						break;

					case "stopped-threads":
						result.StoppedThreads = arg.Value;
						break;

					case "bkptno":
						result.BreakPointNumber = Convert.ToUInt32(arg.Value);
						break;

					case "disp":
						result.KeepBreakPoint = arg.Value == "keep";
						break;

					default:
						//Console.WriteLine ("Unimplemented stop record field detected.");
						break;
				}
			}

			return result;
		}
	}
}