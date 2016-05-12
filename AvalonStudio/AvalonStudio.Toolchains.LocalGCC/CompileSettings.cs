namespace AvalonStudio.Toolchains.LocalGCC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum OptimizationLevel
    {
        None,
        Debug,
        Level1,
        Level2,
        Level3
    }

    public enum OptimizationPreference
    {
        None,
        Speed,
        Size
    }

    public enum FPUSupport
    {
        None,
        Soft,
        Hard
    }

    public class CompileSettings
    {
        public CompileSettings()
        {
            Defines = new List<string>();
            Includes = new List<string>();
            DebugInformation = true;            
        }

        public List<string> Defines { get; set; }
        public List<string> Includes { get; set; }
        public bool DebugInformation { get; set; }
        public bool Rtti { get; set; }
        public bool Exceptions { get; set; }
        public string CustomFlags { get; set; }
        
        public OptimizationLevel Optimization { get; set; }
        public OptimizationPreference OptimizationPreference { get; set; }
        public FPUSupport Fpu { get; set; }
    }
}
