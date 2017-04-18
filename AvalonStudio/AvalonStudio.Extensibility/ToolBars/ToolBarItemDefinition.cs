using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;

namespace AvalonStudio.Extensibility.ToolBars
{
    public abstract class ToolBarItemDefinition
    {
        protected ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display)
        {
            Group = group;
            SortOrder = sortOrder;
            Display = display;
            IoC.RegisterConstant(this);
        }

        public ToolBarItemGroupDefinition Group { get; }

        public int SortOrder { get; }

        public ToolBarItemDisplay Display { get; }

        public string Text { get; }
        public KeyGesture KeyGesture { get; }

        public CommandDefinition CommandDefinition { get; protected set; }

        public virtual void Activation()
        {
        }
    }

    public class ToolBarItemDefinition<TCommand> : ToolBarItemDefinition where TCommand : CommandDefinition
    {
        public ToolBarItemDefinition(ToolBarItemGroupDefinition group, int sortOrder, ToolBarItemDisplay display = ToolBarItemDisplay.IconOnly) : base(group, sortOrder, display)
        {
        }

        public override void Activation()
        {
            base.Activation();

            CommandDefinition = IoC.Get<TCommand>();
        }
    }

    public enum ToolBarItemDisplay
    {
        IconOnly,
        IconAndText
    }
}