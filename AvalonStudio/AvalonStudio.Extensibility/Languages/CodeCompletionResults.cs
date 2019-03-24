using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class CodeCompletionResults
    {
        public CodeCompletionResults()
        {
            Completions = new List<CodeCompletionData>();
        }

        public List<CodeCompletionData> Completions { get; set; }

        public CompletionContext Contexts { get; set; }

        public int? StartOffset { get; set; }
    }
}