namespace AvalonStudio.Extensibility.Commands
{
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Input;
    using Avalonia.Media;
    using Avalonia.Styling;
    using AvalonStudio.Extensibility.Plugin;
    using System.Windows.Input;

    public abstract class CommandDefinition : IExtension
    {
        public CommandDefinition()
        {
        }

        public abstract string Text { get; }
        public abstract string ToolTip { get; }
        public virtual DrawingGroup Icon
        {
            get
            {
                var mainWindow = IoC.Get<Window>();

                var result = mainWindow.FindStyleResource("Light");

                return result as DrawingGroup;
            }
        }

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