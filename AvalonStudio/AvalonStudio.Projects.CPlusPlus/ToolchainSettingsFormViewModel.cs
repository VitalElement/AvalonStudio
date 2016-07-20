using System.Collections.Generic;
using System.Linq;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Toolchains;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class ToolchainSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
	{
		private IList<object> configPages;

		private object selectedConfigPage;

		private IToolChain selectedToolchain;

		private List<IToolChain> toolchains;

		public ToolchainSettingsFormViewModel(CPlusPlusProject project) : base("Toolchain", project)
		{
			toolchains = new List<IToolChain>(IoC.Get<IShell>().ToolChains);
			selectedToolchain = project.ToolChain;
		}

		public IList<object> ConfigPages
		{
			get { return configPages; }
			set { this.RaiseAndSetIfChanged(ref configPages, value); }
		}

		public object SelectedConfigPage
		{
			get { return selectedConfigPage; }
			set { this.RaiseAndSetIfChanged(ref selectedConfigPage, value); }
		}

		public List<IToolChain> Toolchains
		{
			get { return toolchains; }
			set { this.RaiseAndSetIfChanged(ref toolchains, value); }
		}

		public IToolChain SelectedToolchain
		{
			get { return selectedToolchain; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedToolchain, value);

				Save();

				if (value != null)
				{
					ConfigPages = value.GetConfigurationPages(Model);
					SelectedConfigPage = null;
					SelectedConfigPage = ConfigPages.FirstOrDefault();
				}
			}
		}

		public void Save()
		{
			Model.ToolchainReference = selectedToolchain?.GetType().ToString();
			selectedToolchain?.ProvisionSettings(Model);
			Model.Save();
		}
	}
}