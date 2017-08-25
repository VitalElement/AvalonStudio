namespace AvalonStudio.Extensibility.Commands
{
    using Avalonia;
    using Avalonia.Media;
    using Avalonia.Styling;
    using System.Collections.Generic;
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
}