namespace AvalonStudio.Languages
{
    public class CodeCompletionData
    {
        public uint Priority { get; set; }
        public string Suggestion { get; set; }
        public CursorKind Kind { get; set; }
        public string Hint { get; set; }
        public string BriefComment { get; set; }
    }
}
