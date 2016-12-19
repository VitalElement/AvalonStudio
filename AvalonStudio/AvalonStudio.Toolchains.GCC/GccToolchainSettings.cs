namespace AvalonStudio.Toolchains.GCC
{
	public class GccToolchainSettings
	{
		public GccToolchainSettings()
		{
			CompileSettings = new CompileSettings();
			LinkSettings = new LinkSettings();
		}

		public CompileSettings CompileSettings { get; set; }
		public LinkSettings LinkSettings { get; set; }
	}
}