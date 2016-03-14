namespace AvalonStudio.Controls
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using System.Collections.Generic;

    public class LocalsViewModel : ViewModel<List<Variable>>
    {
        public void Clear()
        {
            this.Model = null;
        }

        public LocalsViewModel() : base(new List<Variable>())
        {
        }
    }
}
