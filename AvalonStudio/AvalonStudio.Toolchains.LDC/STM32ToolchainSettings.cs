namespace AvalonStudio.Toolchains.Clang
{
	public class ClangToolchainSettings
	{
		public ClangToolchainSettings()
		{
			CompileSettings = new CompileSettings();
			LinkSettings = new LinkSettings();
		}

		public CompileSettings CompileSettings { get; set; }
		public LinkSettings LinkSettings { get; set; }
	}
}