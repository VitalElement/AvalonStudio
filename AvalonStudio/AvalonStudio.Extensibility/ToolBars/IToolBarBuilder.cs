namespace AvalonStudio.Extensibility.ToolBars
{
    public interface IToolBarBuilder
    {
        void BuildToolBars(IToolBars result);

        void BuildToolBar(ToolBarDefinition toolBarDefinition, IToolBar result);
    }
}