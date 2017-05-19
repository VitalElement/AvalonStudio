using AvalonStudio.Languages.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Languages
{
    public interface IIntellisenseControl
    {
        IList<CompletionDataViewModel> CompletionData { get; set; }
        CompletionDataViewModel SelectedCompletion { get; set; }
        bool IsVisible { get; set; }
    }
}