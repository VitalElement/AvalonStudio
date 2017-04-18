namespace AvalonStudio.Extensibility.ToolBars
{
    public class ToolBarDefinition
    {
        public ToolBarDefinition(int sortOrder, string name)
        {
            SortOrder = sortOrder;
            Name = name;
            IoC.RegisterConstant(this);
        }

        public int SortOrder { get; }

        public string Name { get; }
    }
}