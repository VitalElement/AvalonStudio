using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using AvalonStudio.Languages;
using System.Collections.Generic;

namespace AvalonStudio.Controls.Standard.CodeEditor.Completion
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
