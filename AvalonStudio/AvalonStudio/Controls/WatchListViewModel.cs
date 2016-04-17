namespace AvalonStudio.Controls.ViewModels
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
            Watches = new ObservableCollection<WatchViewModel>();
        }

        private int watchId = 0;

        private ObservableCollection<WatchViewModel> watches;
        public ObservableCollection<WatchViewModel> Watches
        {
            get { return watches; }
            set { this.RaiseAndSetIfChanged(ref watches, value); }
        }

        private IDebugger debugger;
        public IDebugger Debugger { get { return debugger; } }

        public void SetDebugger(IDebugger debugger)
        {
            this.debugger = debugger;
        }

        public void AddWatch(string expression)
        {
            var newWatch = debugger.CreateWatch(string.Format("var{0}", watchId), expression);

            if (newWatch != null)
            {
                watchId++;

                this.Add(newWatch);
            }
        }

        public void RemoveWatch(WatchViewModel watch)
        {
            this.Watches.Remove(watch);

            debugger.DeleteWatch(watch.Model.Id);
        }

        public void Add(VariableObject model)
        {
            var newWatch = new WatchViewModel(this, model);

            newWatch.Evaluate(debugger);

            this.Watches.Add(newWatch);

            //InvalidateColumnWidths();
        }

        private void ApplyChange(VariableObjectChange change)
        {
            foreach (var watch in Watches)
            {
                if (watch.ApplyChange(change))
                {
                    break;
                }
            }
        }

        public virtual void Clear()
        {
            Watches.Clear();
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
