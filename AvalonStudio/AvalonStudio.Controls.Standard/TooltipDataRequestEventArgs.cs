using AvalonStudio.Languages;
using System;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard
{
    public class TooltipDataRequestEventArgs : EventArgs
    {
        public Func<int, Task<object>> GetViewModelAsyncTask { get; set; }
    }
}
