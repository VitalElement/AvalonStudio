using AvalonStudio.Controls;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Extensibility.Languages
{
    public class QuickInfoViewModel : ViewModel<QuickInfoResult>
    {
        public QuickInfoViewModel(QuickInfoResult model) : base(model)
        {
        }

        public StyledText StyledText => Model.Text;
    }
}
