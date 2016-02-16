namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Languages.CPlusPlus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ArmCPlusPlusProjectTemplate : BlankCPlusPlusLangaguageTemplate
    {
        public override string Title
        {
            get
            {
                return "ARM C++ Project"; 
            }
        }

        public override string Description
        {
            get
            {
                return "Basic template for any ARM based device.";
            }
        }
    }
}
