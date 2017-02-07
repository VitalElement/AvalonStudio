using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Composition;

namespace AvalonStudio.Debugging
{
	public class WatchListViewModel : ToolViewModel, IExtension, IWatchList
	{
		protected IDebugManager _debugManager;

		private ObservableCollection<WatchViewModel> children;
		public List<WatchViewModel> LastChangedRegisters;

		public WatchListViewModel()
		{
			Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

			Title = "Watch List";
			Children = new ObservableCollection<WatchViewModel>();
			LastChangedRegisters = new List<WatchViewModel>();

            Activation(); // for when we create the part outside of composition.
        }

		public ObservableCollection<WatchViewModel> Children
		{
			get { return children; }
			set { this.RaiseAndSetIfChanged(ref children, value); }
		}

		public override Location DefaultLocation
		{
			get { return Location.RightMiddle; }
		}

		public virtual void BeforeActivation()
		{
			IoC.RegisterConstant(this, typeof (IWatchList));
		}

		public virtual void Activation()
		{
			_debugManager = IoC.Get<IDebugManager>();

            if (_debugManager != null)
            {
                _debugManager.DebugFrameChanged += WatchListViewModel_DebugFrameChanged;

                _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                _debugManager.DebugSessionEnded += (sender, e) =>
                {
                    IsVisible = false;
                    Clear();
                };
            }
		}

		public async void AddWatch(string expression)
		{
			var newWatch =
				await
					_debugManager.CurrentDebugger.CreateWatchAsync(
						string.Format("var{0}", _debugManager.CurrentDebugger.GetVariableId()), expression);

			if (newWatch != null)
			{
				Add(newWatch);
			}
		}

		public async void RemoveWatch(WatchViewModel watch)
		{
			if (watch != null)
			{
				Dispatcher.UIThread.InvokeAsync(() => { Children.Remove(watch); });

				await _debugManager.CurrentDebugger.DeleteWatchAsync(watch.Model.Id);
			}
		}

		public void AddExistingWatch(VariableObject variable)
		{
			Add(variable);
		}

		public async void Add(VariableObject model)
		{
			var newWatch = new WatchViewModel(this, _debugManager.CurrentDebugger, model);

			await newWatch.Evaluate(_debugManager.CurrentDebugger);

			Dispatcher.UIThread.InvokeAsync(() => { Children.Add(newWatch); });

			//InvalidateColumnWidths();
		}

		private void ApplyChange(VariableObjectChange change)
		{
			foreach (var watch in Children)
			{
				if (watch.ApplyChange(change))
				{
					break;
				}
			}
		}

		public virtual void Clear()
		{
			Children.Clear();
		}

		public async Task Invalidate(List<VariableObjectChange> updates)
		{
			foreach (var watch in LastChangedRegisters)
			{
				await Dispatcher.UIThread.InvokeTaskAsync(() => { watch.HasChanged = false; });
			}

			LastChangedRegisters.Clear();

			if (updates != null)
			{
				foreach (var update in updates)
				{
					ApplyChange(update);
				}
			}
		}

		private async void WatchListViewModel_DebugFrameChanged(object sender, FrameChangedEventArgs e)
		{
			await Invalidate(e.VariableChanges);
		}
	}
}