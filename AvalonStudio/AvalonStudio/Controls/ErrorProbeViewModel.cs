using AvalonStudio.Languages;
using AvalonStudio.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class ErrorProbeViewModel : ViewModel<Diagnostic>
    {
        public ErrorProbeViewModel(Diagnostic model) : base (model)
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
