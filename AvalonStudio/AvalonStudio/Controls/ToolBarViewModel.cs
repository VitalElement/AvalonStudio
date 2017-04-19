using AvalonStudio.Extensibility.ToolBars;
using AvalonStudio.MVVM;
using System.Composition;

namespace AvalonStudio.Controls
{
    public class ToolBarViewModel : ViewModel
    {
        private IToolBarBuilder _toolBarBuilder;

        [ImportingConstructor]
        public ToolBarViewModel(IToolBarBuilder toolBarBuilder)
        {
            _toolBarBuilder = toolBarBuilder;
        }
    }
}