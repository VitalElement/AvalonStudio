using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    class QuickInfoViewModel : ViewModel<QuickInfoResult>
    {
        public QuickInfoViewModel(QuickInfoResult model) : base(model)
        {
        }

        public StyledText StyledText => Model.Text;
    }
}
