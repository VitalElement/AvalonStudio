using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using AvalonStudio.Languages;
using System.Collections.Generic;

namespace AvalonStudio.Controls.Editor.Completion
{
    public static class CodeCompletionKindExtensions
    {
        private static readonly CompletionIconService _service = new CompletionIconService();

        public static DrawingGroup ToDrawingGroup(this CodeCompletionKind kind) => _service.GetCompletionKindImage(kind);

        private class CompletionIconService
        {
            private readonly Dictionary<CodeCompletionKind, DrawingGroup> _cache = new Dictionary<CodeCompletionKind, DrawingGroup>();

            public DrawingGroup GetCompletionKindImage(CodeCompletionKind icon)
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
                        //System.Console.WriteLine($"No intellisense icon provided for {icon}");
                    }
                }

                return image;
            }
        }
    }
}
