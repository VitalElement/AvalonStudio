namespace AvalonStudio.Extensibility.Editor
{
    public class EditorSettings
    {
        public double GlobalZoomLevel { get; set; } = 100;

        public bool RemoveTrailingWhitespaceOnSave { get; set; } = true;

        public bool AutoFormat { get; set; } = true;

        public string ColorScheme { get; set; } = Editor.ColorScheme.Default.Name;
    }
}
