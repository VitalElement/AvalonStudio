namespace AvalonStudio.Extensibility.Menus
{
    public class MenuDefinition
    {
        public MenuDefinition(MenuBarDefinition menuBar, int sortOrder, string text)
        {
            MenuBar = menuBar;
            SortOrder = sortOrder;
            Text = text;
            IoC.RegisterConstant(this);
        }

        public MenuBarDefinition MenuBar { get; protected set; }

        public int SortOrder { get; }

        public string Text { get; }

        public virtual void Activation()
        {
        }
    }
}