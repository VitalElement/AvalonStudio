using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    public class Symbol
    {
        public int SymbolId { get; set; }

        //public int USRForeignKey { get; set; }
        //[InverseProperty(   )]
        public virtual SymbolReference USR { get; set; }

        public int Line { get; set; }
        public int Column { get; set; }
    }
}
