using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.Standard
{
    public class ExportableProperty<T>
    {
        public T Value { get; set; }
        public bool Exported { get; set; }
        public bool Global { get; set; }
    }
}
