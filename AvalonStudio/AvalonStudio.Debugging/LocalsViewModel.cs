using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using Mono.Debugging.Client;

namespace AvalonStudio.Debugging
{
	public class LocalsViewModel : WatchListViewModel, IExtension
	{
		private readonly List<ObjectValue> locals;

		public LocalsViewModel()
		{
			Title = "Locals";
			locals = new List<ObjectValue>();

			Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });
		}

		public override Location DefaultLocation
		{
			get { return Location.RightBottom; }
		}


		public override void Activation()
		{
			_debugManager = IoC.Get<IDebugManager2>();

            if (_debugManager != null)
            {
                _debugManager.TargetStopped += _debugManager_TargetStopped;

                _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                _debugManager.DebugSessionEnded += (sender, e) =>
                {
                    IsVisible = false;
                    Clear();
                };
            }
		}

        private void _debugManager_TargetStopped(object sender, Mono.Debugging.Client.TargetEventArgs e)
        {
            var currentFrame = e.Backtrace.GetFrame(0);

            var locals = currentFrame.GetAllLocals();

            InvalidateLocals(locals);
        }

        public void InvalidateLocals(ObjectValue[] variables)
		{
			var updated = new List<ObjectValue>();
			var removed = new List<ObjectValue>();

			for (var i = 0; i < locals.Count; i++)
			{
				var local = locals[i];

				var currentVar = variables.FirstOrDefault(v => v.Name == local.Name);

				if (currentVar == null)
				{
					removed.Add(local);
				}
				else
				{
					updated.Add(local);
				}
			}

			foreach (var variable in variables)
			{
				var currentVar = updated.FirstOrDefault(v => v.Name == variable.Name);

				if (currentVar == null)
				{
					locals.Add(variable);
                    Add(variable);
				}
                else if(currentVar.CanRefresh)
                {
                    var currentVm = Children.FirstOrDefault(c => c.Model.Name == currentVar.Name);

                    currentVm.Model = variable;

                    currentVm.Invalidate();
                }
			}

			foreach (var removedvar in removed)
			{
				locals.Remove(removedvar);
                Remove(removedvar);
			}
		}

		public override void Clear()
		{
			locals.Clear();
			base.Clear();
		}
	}
}