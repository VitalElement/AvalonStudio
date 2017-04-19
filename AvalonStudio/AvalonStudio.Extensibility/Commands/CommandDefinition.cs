namespace AvalonStudio.Extensibility.Commands
{
    using Avalonia.Controls.Shapes;
    using Avalonia.Input;
    using AvalonStudio.Extensibility.Plugin;
    using System.Windows.Input;

    public abstract class CommandDefinition : IExtension
    {
        public CommandDefinition()
        {
        }

        public abstract string Text { get; }
        public abstract string ToolTip { get; }
        public virtual Path IconPath => null;
        public virtual KeyGesture Gesture => null;
        public abstract ICommand Command { get; }

        public virtual void Activation()
        {
        }

        public virtual void BeforeActivation()
        {
            IoC.RegisterConstant(this, GetType());
        }
    }
}