namespace AvalonStudio.Extensibility.Menus
{
    public abstract class MenuDefinition
    {
        public MenuDefinition(int sortOrder, string text)
        {
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

    public class MenuDefinition<TMenuBar> : MenuDefinition where TMenuBar : MenuBarDefinition
    {
        public MenuDefinition(int sortOrder, string text) : base(sortOrder, text)
        {
            
        }

        public override void Activation()
        {
            base.Activation();

            MenuBar = IoC.Get<TMenuBar>();
        }
    }
}