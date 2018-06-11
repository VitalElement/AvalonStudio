using Avalonia;
using Avalonia.Media;
using System.Collections.Generic;
using System.Composition;

namespace AvalonStudio.Commands
{
    [Export]
    public class CommandIconService
    {
        private readonly Dictionary<string, DrawingGroup> _cache = new Dictionary<string, DrawingGroup>();

        public DrawingGroup GetCompletionKindImage(string icon)
        {
            if (!_cache.TryGetValue(icon, out var image))
            {
                if (Application.Current.Styles.TryGetResource(icon.ToString(), out object resource))
                {
                    image = resource as DrawingGroup;
                    _cache.Add(icon, image);
                }
                else
                {
                    System.Console.WriteLine($"No intellisense icon provided for {icon}");
                }
            }

            return image;
        }
    }
}
