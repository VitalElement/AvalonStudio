namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using Avalonia.Controls;
    using ReactiveUI;
    using Shell;
    using System.Collections.Generic;
    using System.Linq;
    using Toolchains;

    public class ToolchainSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public ToolchainSettingsFormViewModel(CPlusPlusProject project) : base (project)
        {
            toolchains = new List<IToolChain>(IoC.Get<IShell>().ToolChains);
            selectedToolchain = project.ToolChain;            
        }

        public void Save ()
        {
            Model.ToolchainReference = selectedToolchain?.GetType().ToString();
            selectedToolchain?.ProvisionSettings(Model);
            Model.Save();
        }


        private IList<TabItem> configPages;
        public IList<TabItem> ConfigPages
        {
            get { return configPages; }
            set { this.RaiseAndSetIfChanged(ref configPages, value); }
        }

        private TabItem selectedConfigPage;
        public TabItem SelectedConfigPage
        {
            get { return selectedConfigPage; }
            set { this.RaiseAndSetIfChanged(ref selectedConfigPage, value); }
        }

        private List<IToolChain> toolchains;
        public List<IToolChain> Toolchains
        {
            get { return toolchains; }
            set { this.RaiseAndSetIfChanged(ref toolchains, value); }
        }

        private IToolChain selectedToolchain;
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


    }
}
