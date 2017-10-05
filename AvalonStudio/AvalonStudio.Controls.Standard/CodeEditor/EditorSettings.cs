using AvalonStudio.Extensibility.Editor;
using Newtonsoft.Json;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class EditorSettings
    {
        public double GlobalZoomLevel { get; set; } = 100;

        public bool RemoveTrailingWhitespaceOnSave { get; set; } = true;

        public bool AutoFormat { get; set; } = true;

        public string ColorScheme { get; set; } = AvalonStudio.Extensibility.Editor.ColorScheme.Default.Name;
    }
}
