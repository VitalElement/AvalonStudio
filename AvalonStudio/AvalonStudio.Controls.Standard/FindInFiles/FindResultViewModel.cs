using AvalonStudio.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindResultViewModel : ViewModel<FindResult>
    {
        public FindResultViewModel(FindResult model) : base(model)
        {
        }

        public string FilePath => Model.File.Location;

        public int LineNumber => Model.LineNumber;

        public string LineText => Model.LineText;
    }
}
