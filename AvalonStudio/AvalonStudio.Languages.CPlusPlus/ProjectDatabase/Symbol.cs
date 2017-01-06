using System;
namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Symbol
    {
        public int SymbolId { get; set; }
        
        public SymbolReference SymbolReference { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }
    }
}
