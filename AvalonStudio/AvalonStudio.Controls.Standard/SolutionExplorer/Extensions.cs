using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public static class Extensions
    {
        private static readonly CompletionIconService _service = new CompletionIconService();

        public static DrawingGroup ToFileIcon(this string extension) => _service.GetCompletionKindImage("FileIcon" + extension.ToUpper());
        public static DrawingGroup GetIcon(this string resourceName) => _service.GetCompletionKindImage(resourceName);

        public static DrawingGroup GetIcon(this IProject project)
        {
            var name = project.GetType().ToString();

            if (name.EndsWith("CPlusPlusProject"))
            {
                return "CPPIcon".GetIcon();
            }
            else if (name.EndsWith("OmniSharpProject"))
            {
                return "CSharpIcon".GetIcon();
            }
            else if (name.EndsWith("TypeScriptProject"))
            {
                return "TypeScriptIcon".GetIcon();
            }
            else if(name.EndsWith("UnsupportedProjectType"))
            {
                return "UnknownProjectIcon".GetIcon();
            }
            else if (name.EndsWith("LoadingProject"))
            {
                return "LoadingProjectIcon".GetIcon();
            }
            else if (name.EndsWith("NotFoundProject"))
            {
                return "NotFoundProjectIcon".GetIcon();
            }

            return null;
        }

        private class CompletionIconService
        {
            private readonly Dictionary<string, DrawingGroup> _cache = new Dictionary<string, DrawingGroup>();

            public DrawingGroup GetCompletionKindImage(string extension)
            {
                if (!_cache.TryGetValue(extension, out var image))
                {
                    if (Application.Current.Styles.TryGetResource(extension, out object resource))
                    {
                        image = resource as DrawingGroup;
                        _cache.Add(extension, image);
                    }
                    else
                    {
                        //System.Console.WriteLine($"No intellisense icon provided for {extension}");
                    }
                }

                return image;
            }
        }
    }
}
