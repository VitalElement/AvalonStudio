using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    public interface IFindInFilesService
    {
        IEnumerable<FindResult> Find(string searchString, bool caseSensitive, bool wholeWords, bool regex, string[] fileMasks);
    }
}