namespace AvalonStudio.Debugging
{
    using Avalonia.Threading;
    using Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BreakPointManager : List<BreakPoint>
    {
        public BreakPointManager()
        {
        }

        public void SetDebugger(IDebugger debugger)
        {
            if (this.debugger != null)
            {
                this.debugger.Stopped -= debugger_Stopped;
            }

            this.debugger = debugger;

            if (debugger != null)
            {
                this.debugger.Stopped += debugger_Stopped;
            }
        }

        public async void GoLive()
        {
            List<BreakPoint> newList = new List<BreakPoint>();

            foreach (var breakPoint in this)
            {
                var liveBreakPoint = await debugger.SetBreakPointAsync(breakPoint.File, breakPoint.Line);

                if (liveBreakPoint != null)
                {
                    newList.Add(liveBreakPoint);
                }
            }

            this.Clear();
            this.AddRange(newList);
        }

        void debugger_Stopped(object sender, StopRecord e)
        {
            switch (e.Reason)
            {
                case StopReason.Exited:
                case StopReason.ExitedNormally:
                case StopReason.ExitedSignalled:
                    List<BreakPoint> newList = new List<BreakPoint>();

                    foreach (var breakPoint in this)
                    {
                        newList.Add(new BreakPoint() { File = breakPoint.File, Line = breakPoint.Line });
                    }

                    this.Clear();
                    this.AddRange(newList);
                    debugger = null;
                    break;
            }
        }

        new public async Task Add(BreakPoint item)
        {
            if (item == null)
            {
                return;
            }

            if (debugger != null/* && Workspace.Instance.CurrentPerspective == Perspective.Debug*/)
            {
                var liveBreakPoint = item as LiveBreakPoint;

                if (liveBreakPoint == null)
                {
                    liveBreakPoint = await debugger.SetBreakPointAsync(item.File, (uint)item.Line);
                }

                if (liveBreakPoint != null)
                {
                    await Dispatcher.UIThread.InvokeTaskAsync(() =>
                    {
                        base.Add(liveBreakPoint);
                    });
                }
            }
            else
            {
                if (item == null)
                {
                    throw new Exception("Cant be null");
                }

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    base.Add(item);
                });
            }
        }

        new public async Task<bool> Remove(BreakPoint item)
        {
            if (debugger != null && item is LiveBreakPoint)
            {
                await debugger.RemoveAsync(item as LiveBreakPoint);
            }

            return base.Remove(item);
        }

        private IDebugger debugger;
    }
}
