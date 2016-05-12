namespace AvalonStudio.Languages
{
    using AvalonStudio.Languages.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IIntellisenseControl
    {
        Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column, string filter);
        IList<CompletionDataViewModel> CompletionData { get; set; }
        CompletionDataViewModel SelectedCompletion { get; set; }
        bool IsVisible { get; set; }
    }
}
