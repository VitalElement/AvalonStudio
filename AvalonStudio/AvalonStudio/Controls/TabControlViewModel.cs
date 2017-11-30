namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;

    public class TabControlViewModel : ViewModel
    {
        private object selectedTool;
        private bool _tabStripVisible;

        private ObservableCollection<object> tools;

        private Dictionary<ToolViewModel, IDisposable> disposableVisibleObservers;

        public TabControlViewModel()
        {
            tools = new ObservableCollection<object>();
            disposableVisibleObservers = new Dictionary<ToolViewModel, IDisposable>();

            tools.CollectionChanged += (sender, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (var item in e.NewItems)
                        {
                            if (item is ToolViewModel)
                            {
                                var tvm = item as ToolViewModel;

                                var disposable = tvm.IsVisibleObservable.Subscribe(_ => InvalidateIsVisible());

                                disposableVisibleObservers.Add(tvm, disposable);
                            }
                        }

                        InvalidateIsVisible();
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            if (item is ToolViewModel)
                            {
                                IDisposable disposable;

                                var tvm = item as ToolViewModel;

                                if (disposableVisibleObservers.TryGetValue(tvm, out disposable))
                                {
                                    disposable.Dispose();
                                }
                            }
                        }
                        break;
                }
            };
        }

        private void InvalidateIsVisible()
        {
            bool result = false;

            TabStripVisible = Tools.OfType<ToolViewModel>().Where(t => t.IsVisible).Count() > 1;

            foreach (var tool in Tools.OfType<ToolViewModel>())
            {
                if (tool.IsVisible)
                {
                    result = true;
                    break;
                }
            }

            IsVisible = result;
        }        

        public bool TabStripVisible
        {
            get { return _tabStripVisible; }
            set { this.RaiseAndSetIfChanged(ref _tabStripVisible, value); }
        }


        public ObservableCollection<object> Tools
        {
            get { return tools; }
            set { this.RaiseAndSetIfChanged(ref tools, value); }
        }

        public object SelectedTool
        {
            get { return selectedTool; }
            set { this.RaiseAndSetIfChanged(ref selectedTool, value); }
        }

        private bool isVisible;

        public bool IsVisible
        {
            get { return isVisible; }
            set { this.RaiseAndSetIfChanged(ref isVisible, value); }
        }
    }
}