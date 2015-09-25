namespace AvalonStudio.Models.Solutions
{
    using AvalonStudio.Models.Tools.Compiler;
    using System;
    using System.Collections.Generic;

    public enum LibraryType
    {
        None,
        NanoCLib,
        BaseCLib,
        SemiHosting,
        Retarget
    }

    public enum FPUSupport
    {
        None,
        Soft,
        Hard
    }

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

    public class ProjectConfiguration
    {
        public ProjectConfiguration()
        {
            this.ToolChain = new ClangToolChain();
            this.IncludePaths = new List<string>();
            this.Defines = new List<string>();
            this.LinkedLibraries = new List<string>();
            this.DebugSymbols = true;
        }

        public string Name { get; set; }

        private ToolChain toolChain;
        public ToolChain ToolChain
        {
            get { return toolChain; }
            set
            {
                toolChain = value;
            }
        }

        // Compiler Settings
        private bool cppSupport;
        public bool CppSupport
        {
            get { return cppSupport; }
            set
            {

                cppSupport = value;
            }
        }

        public bool DebugSymbols { get; set; }
        public bool Rtti { get; set; }
        public bool Exceptions { get; set; }

        public List<string> IncludePaths { get; set; }

        public List<string> Defines { get; set; }       

        public string MiscCompilerArguments { get; set; }

        // Linker Settings
        public bool UseMemoryLayout { get; set; }
        public bool DiscardUnusedSections { get; set; }
        public bool NotUseStandardStartupFiles { get; set; }

        public FPUSupport Fpu { get; set; }

        public LibraryType Library { get; set; }

        public OptimizationLevel Optimization { get; set; }

        public OptimizationPreference OptimizationPreference { get; set; }

        public List<string> LinkedLibraries { get; set; }

        public string SelectedDeviceName { get; set; }
        public int SelectedDeviceId { get; set; }
        public string Mcpu { get; set; }
        public string March { get; set; }

        public UInt32 InRom1Start { get; set; }
        public UInt32 InRom1Size { get; set; }
        public UInt32 InRom2Start { get; set; }
        public UInt32 InRom2Size { get; set; }
        public UInt32 InRam1Start { get; set; }
        public UInt32 InRam1Size { get; set; }
        public UInt32 InRam2Start { get; set; }
        public UInt32 InRam2Size { get; set; }

        public bool DebugInRam { get; set; }
        public string ScatterFile { get; set; }
        public string MiscLinkerArguments { get; set; }
        public bool IsLibrary { get; set; }       
    }
}
