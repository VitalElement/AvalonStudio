using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    public class SourceFile
    {
        public int SourceFileId { get; set; }
        public string RelativePath { get; set; }
        public DateTime LastModified { get; set; }
    }
}
