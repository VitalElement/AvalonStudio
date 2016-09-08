namespace AvalonStudio.Toolchains.LDC
{
	public class LDCToolchainSettings
	{
		public LDCToolchainSettings()
		{
			CompileSettings = new CompileSettings();
			LinkSettings = new LinkSettings();
		}

		public CompileSettings CompileSettings { get; set; }
		public LinkSettings LinkSettings { get; set; }
	}
}