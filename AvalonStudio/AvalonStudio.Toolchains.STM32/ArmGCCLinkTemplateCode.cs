using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.STM32
{
    partial class ArmGCCLinkTemplate
    {
        public ArmGCCLinkTemplate(LinkSettings linkSettings)
        {
            this.linkSettings = linkSettings;
        }

        private LinkSettings linkSettings;
    }
}
