namespace AvalonStudio.Debugging
{
    using AvalonStudio.MVVM;
    using Debugging;
    using ReactiveUI;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class WatchListViewModel : ViewModel
    {
        public WatchListViewModel()
        {
            Children = new ObservableCollection<WatchViewModel>();
        }

        private ObservableCollection<WatchViewModel> children;
        public ObservableCollection<WatchViewModel> Children
        {
            get { return children; }
            set { this.RaiseAndSetIfChanged(ref children, value); }
        }

        private IDebugger debugger;
        public IDebugger Debugger { get { return debugger; } }

        public void SetDebugger(IDebugger debugger)
        {
            this.debugger = debugger;
        }

        public void AddExistingWatch(VariableObject variable)
        {
            this.Add(variable);
        }

        public void AddWatch(string expression)
        {
            var newWatch = debugger.CreateWatch(string.Format("var{0}", debugger.GetVariableId()), expression);

            if (newWatch != null)
            {
                this.Add(newWatch);
            }
        }

        public void RemoveWatch(WatchViewModel watch)
        {
            if (watch != null)
            {
                this.Children.Remove(watch);

                debugger.DeleteWatch(watch.Model.Id);
            }
        }

        public void Add(VariableObject model)
        {
            var newWatch = new WatchViewModel(debugger, model);

            newWatch.Evaluate(debugger);

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
    }
}
