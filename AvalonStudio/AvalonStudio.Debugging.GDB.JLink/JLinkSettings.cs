namespace AvalonStudio.Debugging.GDB.JLink
{
	public class JLinkSettings
	{
		public JlinkInterfaceType Interface { get; set; }
        public string TargetDevice { get; set; }
        public string DeviceKey { get; set; }
	}
}