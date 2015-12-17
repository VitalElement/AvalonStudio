namespace AvalonStudio.Languages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Diagnostic
    {
        public int Offset { get; set; }
        public int Length { get; set; }
        public string Spelling { get; set; }
    }
}
