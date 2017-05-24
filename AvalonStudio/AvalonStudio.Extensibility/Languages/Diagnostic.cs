using AvalonStudio.Projects;
using AvaloniaEdit.Document;
using System.Collections.Generic;

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

    public enum DiagnosticSource
    {
        Intellisense,
        Build,
        StaticAnalysis
    }

    public class Diagnostic : TextSegment
    {
        public Diagnostic()
        {
            Line = -1;
            Column = -1;

            Children = new List<Diagnostic>();
        }

        public IProject Project { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public ISourceFile File { get; set; }
        public string Spelling { get; set; }
        public DiagnosticLevel Level { get; set; }
        public DiagnosticSource Source { get; set; }

        public List<Diagnostic> Children { get; set; }
    }

    public class FixIt : Diagnostic
    {
        public string ReplacementText { get; set; }
    }
}