using System.Collections.ObjectModel;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
	public class TabControlViewModel : ViewModel
	{
		private object selectedTool;

		private ObservableCollection<object> tools;

		public TabControlViewModel()
		{
			tools = new ObservableCollection<object>();
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
	}
}