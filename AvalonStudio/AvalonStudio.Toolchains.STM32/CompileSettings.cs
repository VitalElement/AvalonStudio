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
            Includes = new List<string>();
        }

        public List<string> Defines { get; set; }
        public List<string> Includes { get; set; }
        public bool DebugInformation { get; set; }
        public bool Rtti { get; set; }
        public bool Exceptions { get; set; }
    }
}
