using System.ComponentModel;

namespace AvalonStudio.Debugging.GDB.Remote
{
    public class RemoteGdbSettings
    {
        public string PreInitCommand { get; set; }
        public string PreInitCommandArgs { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string GDBInitCommands { get; set; }
        public string PostInitCommand { get; set; }
        public string PostInitCommandArgs { get; set; }
        public string GDBExitCommands { get; set; }
    }
}