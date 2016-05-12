namespace AvalonStudio.Controls
{
    using Extensibility.ToolBars;
    using MVVM;
    using System.ComponentModel.Composition;
    public class ToolBarViewModel : ViewModel, IPartImportsSatisfiedNotification
    {
        private IToolBarBuilder toolBarBuilder;

        [ImportingConstructor]
        public ToolBarViewModel(IToolBarBuilder toolBarBuilder)
        {
            this.toolBarBuilder = toolBarBuilder;

            
        }        

        public void OnImportsSatisfied()
        {
         //   toolBarBuilder.BuildToolBar(ToolBarDefinitions.MainToolBar, 
        }
    }
}
