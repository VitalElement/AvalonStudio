using AvalonStudio.Documents;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class SymbolRenameInfo
    {
        public SymbolRenameInfo(string filePath)
        {
            FileName = filePath;
        }

        public string FileName { get; }

        public IEnumerable<LinePositionSpanTextChange> Changes { get; set; }

        public IEnumerable<(TextLocation start, TextLocation end)> InitialLocations { get; set; }
    }
}
