using AvalonStudio.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Extensibility.Languages.CompletionAssistance
{
    public class MethodInfo
    {
        public MethodInfo (List<Symbol> overloads, int offset)
        {
            Overloads = overloads;
            Offset = offset;
        }

        public int Offset { get; private set; }

        public List<Symbol> Overloads { get; private set; }
    }
}
