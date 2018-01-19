using System;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public class TooltipDataRequestEventArgs
    {
        public Func<int, Task<object>> GetViewModelAsyncTask { get; set; }
    }
}