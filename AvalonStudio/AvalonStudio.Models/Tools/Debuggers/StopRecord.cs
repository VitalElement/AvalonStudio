using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Tools.Debuggers
{
    public enum StopReason
    {
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
        public static StopRecord FromArgumentList (NameValuePair [] arguments)
        {
            StopRecord result = new StopRecord ();

            foreach (NameValuePair arg in arguments)
            {
                switch (arg.Name)
                {
                    case "reason":
                        result.Reason = arg.Value.ToStopReason ();
                        break;

                    case "frame":
                        result.Frame = Frame.FromDataString (arg.Value);
                        break;

                    case "thread-id":
                        result.ThreadId = Convert.ToUInt32 (arg.Value);
                        break;

                    case "stopped-threads":
                        result.StoppedThreads = arg.Value;
                        break;

                    case "bkptno":
                        result.BreakPointNumber = Convert.ToUInt32 (arg.Value);
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

        public StopReason Reason { get; set; }
        public Frame Frame { get; set; }
        public UInt32 ThreadId { get; set; }
        public UInt32 BreakPointNumber { get; set; }
        public bool KeepBreakPoint { get; set; }
        public string StoppedThreads { get; set; }
    }
}
