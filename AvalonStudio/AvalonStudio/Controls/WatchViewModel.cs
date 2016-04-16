namespace AvalonStudio.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using MVVM;
    using Debugging;

    public class WatchViewModel : ViewModel<VariableObject>
    {
        public WatchViewModel(VariableObject variable) : base(variable)
        {

        }
    }
}
