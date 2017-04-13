using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Composition;
using Mono.Debugging.Client;
using System;
using System.Linq;

namespace AvalonStudio.Debugging
{
	public class WatchListViewModel : ToolViewModel, IExtension, IWatchList
	{
		protected IDebugManager2 _debugManager;

		private ObservableCollection<ObjectValueViewModel> children;
		public List<ObjectValueViewModel> LastChangedRegisters;

		public WatchListViewModel()
		{
			Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

			Title = "Watch List";
			Children = new ObservableCollection<ObjectValueViewModel>();
			LastChangedRegisters = new List<ObjectValueViewModel>();

            Activation(); // for when we create the part outside of composition.
        }

		public ObservableCollection<ObjectValueViewModel> Children
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
			_debugManager = IoC.Get<IDebugManager2>();

            if (_debugManager != null)
            {
                //_debugManager.TargetStopped += _debugManager_TargetStopped;
                //_debugManager.DebugFrameChanged += WatchListViewModel_DebugFrameChanged;

                _debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

                _debugManager.DebugSessionEnded += (sender, e) =>
                {
                    IsVisible = false;
                    Clear();
                };
            }
		}

		public void Add(ObjectValue model)
		{
			var newWatch = new ObjectValueViewModel(this, model);

            Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                Children.Add(newWatch);
            }).Wait();
		}

        public void Remove(ObjectValue value)
        {
            Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                Children.Remove(Children.FirstOrDefault(c => c.Model == value));
            }).Wait();
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