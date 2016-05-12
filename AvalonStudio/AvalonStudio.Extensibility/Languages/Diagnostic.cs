using AvalonStudio.Projects;

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

    public class Diagnostic
    {
        public IProject Project { get; set; }
        public int Offset { get; set; }
        public int Line { get; set; }
        public int Length { get; set; }
        public string File { get; set; }        
        public string Spelling { get; set; }
        public DiagnosticLevel Level { get; set; }
    }
}
