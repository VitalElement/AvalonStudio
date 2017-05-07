using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CorApi.Portable
{
    // [Xamarin] Output redirection.
    public class CorTargetOutputEventArgs : EventArgs
    {
        public CorTargetOutputEventArgs(string text, bool isStdError)
        {
            Text = text;
            IsStdError = isStdError;
        }

        public string Text { get; set; }

        public bool IsStdError { get; set; }
    }

    public delegate void CorTargetOutputEventHandler(Object sender, CorTargetOutputEventArgs e);

    public static class CorProcessExtensions
    {
        internal static void TrackStdOutput(this Process proc, Microsoft.Win32.SafeHandles.SafeFileHandle outputPipe, Microsoft.Win32.SafeHandles.SafeFileHandle errorPipe)
        {
            throw new NotImplementedException();
            /*var outputReader = new Thread(delegate ()
            {
                
            });
            outputReader.Name = "Debugger output reader";
            outputReader.IsBackground = true;
            outputReader.Start();

            var errorReader = new Thread(delegate ()
            {
                proc.ReadOutput(errorPipe, true);
            });
            errorReader.Name = "Debugger error reader";
            errorReader.IsBackground = true;
            errorReader.Start();*/
        }

        // [Xamarin] Output redirection.
        static void ReadOutput(this Process proc, Microsoft.Win32.SafeHandles.SafeFileHandle pipe, bool isStdError)
        {
            throw new NotImplementedException();
            var buffer = new byte[256];
            int nBytesRead;

            try
            {
                while (true)
                {
                    /*if (!DebuggerExtensions.ReadFile(pipe, buffer, buffer.Length, out nBytesRead, IntPtr.Zero) || nBytesRead == 0)
                        break; // pipe done - normal exit path.*/

                    string s = System.Text.Encoding.ASCII.GetString(buffer, 0, nBytesRead);
                    List<CorTargetOutputEventHandler> list;
                    if (events.TryGetValue(proc, out list))
                        foreach (var del in list)
                            del(proc, new CorTargetOutputEventArgs(s, isStdError));
                }
            }
            catch
            {
            }
        }

        public static void RegisterStdOutput(this Process proc, CorTargetOutputEventHandler handler)
        {
            proc.OnProcessExit += delegate
            {
                RemoveEventsFor(proc);
            };

            List<CorTargetOutputEventHandler> list;
            if (!events.TryGetValue(proc, out list))
                list = new List<CorTargetOutputEventHandler>();
            list.Add(handler);

            events[proc] = list;
        }

        static void RemoveEventsFor(Process proc)
        {
            events.Remove(proc);
        }

        // [Xamarin] Output redirection.
        static readonly Dictionary<Process, List<CorTargetOutputEventHandler>> events = new Dictionary<Process, List<CorTargetOutputEventHandler>>();
    }
}
