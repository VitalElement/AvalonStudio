namespace AvalonStudio.Debugging
{
    using AvalonStudio.MVVM;
    using Debugging;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System;
    using Extensibility.Plugin;
    using Extensibility;
    using Avalonia.Threading;
    public class WatchListViewModel : ToolViewModel, IExtension
    {
        protected IDebugManager _debugManager;

        public WatchListViewModel()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                IsVisible = false;
            });

            Title = "Watch List";
            Children = new ObservableCollection<WatchViewModel>();
        }

        public WatchListViewModel(IDebugManager debugManager) : this()
        {
            _debugManager = IoC.Get<IDebugManager>();
            //_debugManager.DebugFrameChanged += WatchListViewModel_DebugFrameChanged;

            //_debugManager.DebugSessionStarted += (sender, e) =>
            //{
            //    IsVisible = true;
            //};

            //_debugManager.DebugSessionEnded += (sender, e) =>
            //{
            //    IsVisible = false;
            //    Clear();
            //};
        }

        private ObservableCollection<WatchViewModel> children;
        public ObservableCollection<WatchViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.RightMiddle;
            }
        }

        public void AddExistingWatch(VariableObject variable)
        {
            this.Add(variable);
        }

        public async void AddWatch(string expression)
        {
            var newWatch = await _debugManager.CurrentDebugger.CreateWatchAsync(string.Format("var{0}", _debugManager.CurrentDebugger.GetVariableId()), expression);

            if (newWatch != null)
            {
                this.Add(newWatch);
            }
        }

        public async void RemoveWatch(WatchViewModel watch)
        {
            if (watch != null)
            {
                this.Children.Remove(watch);

                await _debugManager.CurrentDebugger.DeleteWatchAsync(watch.Model.Id);
            }
        }

        public async void Add(VariableObject model)
        {
            var newWatch = new WatchViewModel(_debugManager.CurrentDebugger, model);

            await newWatch.Evaluate(_debugManager.CurrentDebugger);

            this.Children.Add(newWatch);

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

        public void Invalidate(List<VariableObjectChange> updates)
        {
            if (updates != null)
            {
                foreach (var update in updates)
                {
                    ApplyChange(update);
                }
            }
        }

        public virtual void BeforeActivation()
        {

        }

        public virtual void Activation()
        {
            _debugManager = IoC.Get<IDebugManager>();
            _debugManager.DebugFrameChanged += WatchListViewModel_DebugFrameChanged;

            _debugManager.DebugSessionStarted += (sender, e) =>
            {
                IsVisible = true;
            };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
                Clear();
            };
        }

        private void WatchListViewModel_DebugFrameChanged(object sender, FrameChangedEventArgs e)
        {
            Invalidate(e.VariableChanges);
        }
    }
}
