using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.LanguageServices
{
    public class UnsavedFile
    {
        public UnsavedFile (string filename, string contents)
        {
            this.FileName = filename;
            this.Contents = contents;
        }

        public readonly string FileName;
        public string Contents;
    }
}
