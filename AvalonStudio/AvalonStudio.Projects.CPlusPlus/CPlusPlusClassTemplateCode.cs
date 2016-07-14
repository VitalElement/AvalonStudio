using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.CPlusPlus
{
    partial class CPlusPlusClassTemplate
    {
        public CPlusPlusClassTemplate(string className, bool withHeader)
        {
            this.className = className;
            this.withHeader = withHeader;
        }

        private bool withHeader;
        private string className;
    }
}