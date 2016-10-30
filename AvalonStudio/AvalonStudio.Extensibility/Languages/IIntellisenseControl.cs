using System.Collections.Generic;
using System.Threading.Tasks;
using AvalonStudio.Languages.ViewModels;

namespace AvalonStudio.Languages
{
	public interface IIntellisenseControl
	{
		IList<CompletionDataViewModel> CompletionData { get; set; }
		CompletionDataViewModel SelectedCompletion { get; set; }
		bool IsVisible { get; set; }
		Task<CodeCompletionResults> DoCompletionRequestAsync(int index, int line, int column);
	}
}