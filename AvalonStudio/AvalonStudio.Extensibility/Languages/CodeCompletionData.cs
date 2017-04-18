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
        EnumConstant,
        Struct,
        Class,
        Method,
        Field,
        Parameter,
        None,
        Macro,
        Variable,
        OverloadCandidate
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