using System;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public class TooltipDataRequestEventArgs
    {
        public TooltipDataRequestEventArgs(ITextEditor editor, int offset)
        {
            Editor = editor;
            Offset = offset;
        }

        public ITextEditor Editor { get; }
        public int Offset { get; }

        public Func<ITextEditor, int, Task<object>> GetViewModelAsyncTask { get; set; }
    }
}