namespace AvalonStudio.Languages
{
    public enum CodeCompletionKind
    {
        Keyword,
        Namespace,
        Delegate,
        Interface,
        Property,
        Event,
        Enum,
        Struct,
        Class,
        Method,
        None,
        Macro,

    }

	public class CodeCompletionData
	{
		public uint Priority { get; set; }
		public string Suggestion { get; set; }
		public CodeCompletionKind Kind { get; set; }
		public string Hint { get; set; }
		public string BriefComment { get; set; }
	}
}