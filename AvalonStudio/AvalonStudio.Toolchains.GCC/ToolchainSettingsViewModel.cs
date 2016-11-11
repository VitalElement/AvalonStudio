using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Toolchains.GCC
{
	public class ToolchainSettingsViewModel : ViewModel<IProject>
	{
		private CompileSettingsFormViewModel compileSettings;

		private LinkerSettingsFormViewModel linkerSettings;

		public ToolchainSettingsViewModel(IProject model) : base(model)
		{
			CompileSettings = new CompileSettingsFormViewModel(model);
			LinkerSettings = new LinkerSettingsFormViewModel(model);
		}

		public CompileSettingsFormViewModel CompileSettings
		{
			get { return compileSettings; }
			set { this.RaiseAndSetIfChanged(ref compileSettings, value); }
		}

		public LinkerSettingsFormViewModel LinkerSettings
		{
			get { return linkerSettings; }
			set { this.RaiseAndSetIfChanged(ref linkerSettings, value); }
		}
	}
}