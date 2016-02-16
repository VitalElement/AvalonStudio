namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Languages.CPlusPlus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class STM32CPlusPlusProjectTemplate : BlankCPlusPlusLangaguageTemplate
    {
        public override string DefaultProjectName
        {
            get
            {
                return "STM32Project";
            }
        }

        public override string Title
        {
            get
            {
                return "STM32 C++ Project";
            }
        }

        public override string Description
        {
            get
            {
                return "Basic template for STM32 based devices. Includes startup code and peripheral libraries.";
            }
        }
    }
}
