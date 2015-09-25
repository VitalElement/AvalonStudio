using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Models.Solutions
{
    public class HiddenProject : Project
    {
        private HiddenProject() : base ()
        {

        }

        public HiddenProject(Solution solution, Item container)
            : base(solution, container)
        {

        }
    }
}
