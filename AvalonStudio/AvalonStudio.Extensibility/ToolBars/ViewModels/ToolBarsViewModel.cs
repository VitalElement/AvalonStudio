using System.ComponentModel.Composition;
using AvalonStudio.Extensibility.MVVM;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Extensibility.ToolBars.ViewModels
{
	[Export(typeof (IToolBar))]
	public class ToolBarsViewModel : ViewModel, IToolBars, IPartImportsSatisfiedNotification
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


		public void OnImportsSatisfied()
		{
			_toolBarBuilder.BuildToolBars(this);

			// TODO: Ideally, the ToolBarTray control would expose ToolBars
			// as a dependency property. We could use an attached property
			// to workaround this. But for now, toolbars need to be
			// created prior to the following code being run.
			//foreach (var toolBar in Items)
			//    ((IToolBarsView)view).ToolBarTray.ToolBars.Add(new MainToolBar
			//    {
			//        ItemsSource = toolBar
			//    });            
		}

		public IObservableCollection<IToolBar> Items => _items;

		public bool Visible
		{
			get { return _visible; }
			set { this.RaiseAndSetIfChanged(ref _visible, value); }
		}
	}
}