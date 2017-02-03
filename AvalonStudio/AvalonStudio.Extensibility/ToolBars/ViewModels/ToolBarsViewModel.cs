using System.Composition;
using AvalonStudio.Extensibility.MVVM;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Extensibility.ToolBars.ViewModels
{
	[Export(typeof (IToolBars))]
	public class ToolBarsViewModel : ViewModel, IToolBars
	{
		private readonly BindableCollection<IToolBar> _items;

		private readonly IToolBarBuilder _toolBarBuilder;

		private bool _visible;

		[ImportingConstructor]
		public ToolBarsViewModel(IToolBarBuilder toolBarBuilder)
		{
			_toolBarBuilder = toolBarBuilder;
			_items = new BindableCollection<IToolBar>();
		}

		public IObservableCollection<IToolBar> Items => _items;

		public bool Visible
		{
			get { return _visible; }
			set { this.RaiseAndSetIfChanged(ref _visible, value); }
		}
	}
}