using AvalonStudio.Languages.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Languages
{
    public interface IIntellisenseControl
    {
        IList<CodeCompletionData> CompletionData { get; set; }
        CodeCompletionData SelectedCompletion { get; set; }
        bool IsVisible { get; set; }

        Task<CodeCompletionResults> DoCompletionRequestAsync(int index, int line, int column);
    }
}