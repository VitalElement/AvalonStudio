namespace AvalonStudio.Controls
{
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.ObjectModel;

    public class TabControlViewModel : ViewModel
    {
        public TabControlViewModel()
        {
            tools = new ObservableCollection<object>();
        }

        private ObservableCollection<object> tools;
        public ObservableCollection<object> Tools
        {
            get { return tools; }
            set { this.RaiseAndSetIfChanged(ref tools, value); }
        }

        private object selectedTool;
        public object SelectedTool
        {
            get { return selectedTool; }
            set { this.RaiseAndSetIfChanged(ref selectedTool, value); }
        }
    }
}