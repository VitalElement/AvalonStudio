using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace AvalonStudio.Debugging
{
	public class BreakPointManager : List<BreakPoint>
	{
		private IDebugger debugger;

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

		public async Task GoLiveAsync()
		{
			var newList = new List<BreakPoint>();

			foreach (var breakPoint in this)
			{
				var liveBreakPoint = await debugger.SetBreakPointAsync(breakPoint.File, breakPoint.Line);

				if (liveBreakPoint != null)
				{
					newList.Add(liveBreakPoint);
				}
			}

			Clear();
			AddRange(newList);
		}

		private void debugger_Stopped(object sender, StopRecord e)
		{
			switch (e.Reason)
			{
				case StopReason.Exited:
				case StopReason.ExitedNormally:
				case StopReason.ExitedSignalled:
					var newList = new List<BreakPoint>();

					foreach (var breakPoint in this)
					{
						newList.Add(new BreakPoint {File = breakPoint.File, Line = breakPoint.Line});
					}

					Clear();
					AddRange(newList);
					debugger = null;
					break;
			}
		}

		public new async Task Add(BreakPoint item)
		{
			if (item == null)
			{
				return;
			}

			if (debugger != null /* && Workspace.Instance.CurrentPerspective == Perspective.Debug*/)
			{
				var liveBreakPoint = item as LiveBreakPoint;

				if (liveBreakPoint == null)
				{
					liveBreakPoint = await debugger.SetBreakPointAsync(item.File, item.Line);
				}

				if (liveBreakPoint != null)
				{
					await Dispatcher.UIThread.InvokeTaskAsync(() => { base.Add(liveBreakPoint); });
				}
			}
			else
			{
				if (item == null)
				{
					throw new Exception("Cant be null");
				}

				await Dispatcher.UIThread.InvokeTaskAsync(() => { base.Add(item); });
			}
		}

		public new async Task<bool> Remove(BreakPoint item)
		{
			if (debugger != null && item is LiveBreakPoint)
			{
				await debugger.RemoveAsync(item as LiveBreakPoint);
			}

			return base.Remove(item);
		}
	}
}