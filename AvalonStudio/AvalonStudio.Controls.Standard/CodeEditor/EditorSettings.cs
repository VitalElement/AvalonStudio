using Avalonia.Media;
using AvalonStudio.Extensibility.Editor;
using Newtonsoft.Json;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class EditorSettings
    {
        public EditorSettings()
        {
            GlobalZoomLevel = 100;

            ColorScheme = ColorScheme.Default;
        }

        public double GlobalZoomLevel { get; set; }

        [JsonIgnore]
        public ColorScheme ColorScheme { get; set; }
    }
}
