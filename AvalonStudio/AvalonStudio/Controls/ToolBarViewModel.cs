using System.ComponentModel.Composition;
using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls
{
	public class ToolBarViewModel : ViewModel, IPartImportsSatisfiedNotification
	{
		private IToolBarBuilder _toolBarBuilder;

		[ImportingConstructor]
		public ToolBarViewModel(IToolBarBuilder toolBarBuilder)
		{
			_toolBarBuilder = toolBarBuilder;
		}

		public void OnImportsSatisfied()
		{
			//   toolBarBuilder.BuildToolBar(ToolBarDefinitions.MainToolBar, 
		}
	}
}