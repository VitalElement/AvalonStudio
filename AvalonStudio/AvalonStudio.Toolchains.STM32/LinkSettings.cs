namespace AvalonStudio.Toolchains.STM32
{
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

    public class LinkSettings
    {        
        public LinkSettings()
        {
            LinkedLibraries = new List<string>();
        }

        public List<string> LinkedLibraries { get; set; }

        public bool UseMemoryLayout { get; set; }
        public bool DiscardUnusedSections { get; set; }
        public bool NotUseStandardStartupFiles { get; set; }        

        public LibraryType Library { get; set; }

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
    }    
}
