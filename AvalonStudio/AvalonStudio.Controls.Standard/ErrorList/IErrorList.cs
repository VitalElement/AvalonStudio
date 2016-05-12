namespace AvalonStudio.Controls.Standard.ErrorList
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IErrorList
    {
        ObservableCollection<ErrorViewModel> Errors { get; }
    }
}
