namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SymbolReference
    {
        public int SymbolReferenceId { get; set; }

        public string Reference { get; set; }

        public int? DefinitionForeignKey { get; set; }
        public Symbol Definition { get; set; }
        
        public List<Symbol> Symbols { get; set; }
    }
}
