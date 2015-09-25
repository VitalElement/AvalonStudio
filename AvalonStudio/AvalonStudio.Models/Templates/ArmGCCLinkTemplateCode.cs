using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Models.Solutions;

namespace AvalonStudio.Models.Templates
{
    partial class ArmGCCLinkTemplate
    {
        public ArmGCCLinkTemplate(ProjectConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private ProjectConfiguration configuration;
    }
}
