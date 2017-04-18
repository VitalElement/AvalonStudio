using System.Collections.Generic;

namespace AvalonStudio.Toolchains.GCC
{
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
            LinkerScripts = new List<string>();
        }

        public List<string> LinkedLibraries { get; set; }
        public List<string> LinkerScripts { get; set; }

        public bool UseMemoryLayout { get; set; }
        public bool DiscardUnusedSections { get; set; }
        public bool NotUseStandardStartupFiles { get; set; }

        public LibraryType Library { get; set; }

        public string SelectedDeviceName { get; set; }
        public int SelectedDeviceId { get; set; }
        public string Mcpu { get; set; }
        public string March { get; set; }

        public uint InRom1Start { get; set; }
        public uint InRom1Size { get; set; }
        public uint InRom2Start { get; set; }
        public uint InRom2Size { get; set; }
        public uint InRam1Start { get; set; }
        public uint InRam1Size { get; set; }
        public uint InRam2Start { get; set; }
        public uint InRam2Size { get; set; }

        public bool DebugInRam { get; set; }
        public string ScatterFile { get; set; }
        public string MiscLinkerArguments { get; set; }
    }
}