using System.Collections.Generic;

namespace AvalonStudio.Controls.Editor.Completion
{
    public interface IIntellisenseControl
    {
        IList<CompletionDataViewModel> CompletionData { get; set; }
        CompletionDataViewModel SelectedCompletion { get; set; }
        bool IsVisible { get; set; }
    }
}