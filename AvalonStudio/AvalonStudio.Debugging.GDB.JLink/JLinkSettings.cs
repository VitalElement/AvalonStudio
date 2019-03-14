using System.ComponentModel;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public enum JlinkInterfaceType
    {
        [Description("JTAG")] JTAG,
        [Description("SWD")] SWD,
        [Description("FINE")] FINE,
        [Description("2-wire-JTAG-PIC32")] JTAGPic32
    }

    public class JLinkSettings
    {
        public JLinkSettings()
        {
        }

        public string Version { get; set; }
        public JlinkInterfaceType Interface { get; set; }
        public string TargetDevice { get; set; }
        public string DeviceKey { get; set; }
        public int SpeedkHz { get; set; }

        public bool PostDownloadReset { get; set; } = true;

        public bool Download { get; set; } = true;

        public bool Reset { get; set; } = true;

        public bool Run { get; set; } = true;

        public bool UseRemote { get; set; } = false;

        public string RemoteIPAddress { get; set; } = "";
    }
}