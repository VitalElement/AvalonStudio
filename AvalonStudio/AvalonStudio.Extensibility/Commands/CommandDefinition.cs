namespace AvalonStudio.Extensibility.Commands
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Controls.Shapes;
    using Avalonia.Input;
    using Avalonia.Media;
    using Avalonia.Styling;
    using AvalonStudio.Extensibility.Plugin;
    using System.Collections.Generic;
    using System.Windows.Input;

    public static class CommandDefinitionExtensions
    {
        private static readonly CommandIconService _service = new CommandIconService();

        public static DrawingGroup GetCommandIcon(this CommandDefinition instance, string kind) => _service.GetCompletionKindImage(kind);

        private class CommandIconService
        {
            private readonly Dictionary<string, DrawingGroup> _cache = new Dictionary<string, DrawingGroup>();

            public DrawingGroup GetCompletionKindImage(string icon)
            {
                if (!_cache.TryGetValue(icon, out var image))
                {
                    var resource = Application.Current.FindStyleResource(icon.ToString());

                    if (resource == AvaloniaProperty.UnsetValue)
                    {
                        System.Console.WriteLine($"No intellisense icon provided for {icon}");
                    }
                    else
                    {
                        image = resource as DrawingGroup;
                        _cache.Add(icon, image);
                    }
                }

                return image;
            }
        }
    }

    public abstract class CommandDefinition : IExtension
    {
        public CommandDefinition()
        {
        }

        public abstract string Text { get; }
        public abstract string ToolTip { get; }
        public virtual DrawingGroup Icon => null;

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