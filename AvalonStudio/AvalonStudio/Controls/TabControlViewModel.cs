namespace AvalonStudio.Controls
{
    using System.Collections.ObjectModel;
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.Specialized;
    using System;

    public class TabControlViewModel : ViewModel
	{
		private object selectedTool;

		private ObservableCollection<object> tools;        

		public TabControlViewModel()
		{
			tools = new ObservableCollection<object>();

            tools.CollectionChanged += (sender, e) =>
            {
                switch(e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach(var item in e.NewItems)
                        {
                            if(item is ToolViewModel)
                            {
                                var tvm = item as ToolViewModel;

                                tvm.IsVisibleObservable.Subscribe(_ => InvalidateIsVisible());                                                                
                            }
                        }

                        InvalidateIsVisible();
                        break;
                }
                
            };
		}
        
        private void InvalidateIsVisible()
        {
            bool result = false;

            foreach(var tool in Tools)
            {
                if(tool is ToolViewModel)
                {
                    if((tool as ToolViewModel).IsVisible)
                    {
                        result = true;
                        break;
                    }
                }
            }

            IsVisible = result;
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