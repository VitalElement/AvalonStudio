using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    public class Symbol
    {
        public int SymbolId { get; set; }
        public virtual Definition Definition { get; set; }
        public virtual SymbolReference USR { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
