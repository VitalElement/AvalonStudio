using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls
{
    public class ErrorProbeViewModel : ViewModel<Diagnostic>
    {
        public ErrorProbeViewModel(Diagnostic model) : base(model)
        {
        }

        public string Tooltip
        {
            get
            {
                return Model.Spelling;
            }
        }
    }
}