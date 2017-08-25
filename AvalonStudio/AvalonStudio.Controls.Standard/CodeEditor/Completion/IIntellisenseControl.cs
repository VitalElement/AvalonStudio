using System.Collections.Generic;

namespace AvalonStudio.Controls.Standard.CodeEditor.Completion
{
    public interface IIntellisenseControl
    {
        IList<CompletionDataViewModel> CompletionData { get; set; }
        CompletionDataViewModel SelectedCompletion { get; set; }
        bool IsVisible { get; set; }
    }
}