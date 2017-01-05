using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    public class SymbolReference
    {
        public int SymbolReferenceId { get; set; }        
        public string Reference { get; set; }

        //public int DefinitionForeignKey { get; set; }
        //[InverseProperty("USR")]
        public virtual Symbol Definition { get; set; }

        [InverseProperty("USR")]
        public virtual List<Symbol> Symbols { get; set; }
    }
}
