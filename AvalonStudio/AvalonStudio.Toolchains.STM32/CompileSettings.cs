namespace AvalonStudio.Toolchains.STM32
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CompileSettings
    {
        public CompileSettings()
        {
            Defines = new List<string>();
        }

        public List<string> Defines { get; set; }
    }
}
