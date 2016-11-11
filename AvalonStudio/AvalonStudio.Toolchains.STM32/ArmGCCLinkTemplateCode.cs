using AvalonStudio.Toolchains.GCC;

namespace AvalonStudio.Toolchains.STM32
{
	partial class ArmGCCLinkTemplate
	{
		private readonly LinkSettings linkSettings;

		public ArmGCCLinkTemplate(LinkSettings linkSettings)
		{
			this.linkSettings = linkSettings;
		}
	}
}