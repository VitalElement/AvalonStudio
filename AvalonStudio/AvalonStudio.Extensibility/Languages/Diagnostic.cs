using AvalonStudio.Projects;
using AvaloniaEdit.Document;
using Avalonia.Media;

namespace AvalonStudio.Languages
{
    public enum DiagnosticLevel
    {
        Ignored = 0,
        Note = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4
    }

    public class Diagnostic : TextSegment
    {
        public static readonly Color ErrorBrush = Color.FromRgb(253, 45, 45);
        public static readonly Color WarningBrush = Color.FromRgb(255, 207, 40);
        public static readonly Color DefaultBrush = Color.FromRgb(0, 42, 74);

        public IProject Project { get; set; }
        public int Line { get; set; }
        public string File { get; set; }
        public string Spelling { get; set; }
        public DiagnosticLevel Level { get; set; }
    }
}