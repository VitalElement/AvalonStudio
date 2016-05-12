namespace AvalonStudio.Languages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CodeCompletionResults
    {
        public CodeCompletionResults()
        {
            Completions = new List<CodeCompletionData>();
        }

        public List<CodeCompletionData> Completions { get; set; }
    }
}
