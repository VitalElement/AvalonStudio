namespace AvalonStudio.Languages
{
    public class CodeCompletionData
    {
        public uint Priority { get; set; }
        public string Suggestion { get; set; }
        public CursorKind Kind { get; set; }
    }
}
