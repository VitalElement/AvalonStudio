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
        public MethodInfo (List<Symbol> overloads)
        {
            Overloads = overloads;
        }

        public List<Symbol> Overloads { get; private set; }
    }
}
