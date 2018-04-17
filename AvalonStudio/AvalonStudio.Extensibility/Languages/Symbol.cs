using System;
using System.Collections.Generic;

namespace AvalonStudio.Languages
{
    public class Symbol : IComparable<Symbol>
    {
        public string Name { get; set; }
        public CursorKind Kind { get; set; }
        public LinkageKind Linkage { get; set; }
        public AccessType Access { get; set; }
        public string BriefComment { get; set; }
        public string TypeDescription { get; set; }
        public string EnumDescription { get; set; }
        public string Definition { get; set; }
        public string SymbolType { get; set; }
        public string ResultType { get; set; }

        public bool IsBuiltInType { get; set; }
        public IList<ParameterSymbol> Arguments { get; set; }
        public bool IsVariadic { get; set; }
        public bool IsAsync { get; set; }

        public int CompareTo(Symbol other)
        {
            var result = Kind.CompareTo(other.Kind);

            if (result == 0)
            {
                result = Name.CompareTo(other.Name);
            }

            return result;
        }
    }
}