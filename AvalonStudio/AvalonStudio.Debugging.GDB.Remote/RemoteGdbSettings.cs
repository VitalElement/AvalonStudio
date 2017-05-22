using System.ComponentModel;

namespace AvalonStudio.Debugging.GDB.Remote
{
    public class RemoteGdbSettings
    {
        public string Port { get; set; }
        public string GDBInitCommands { get; set; }
    }
}