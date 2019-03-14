using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Toolchains;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ToolchainSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        private IList<object> configPages;

        private object selectedConfigPage;

        private IToolchain selectedToolchain;

        private List<IToolchain> toolchains;

        public ToolchainSettingsFormViewModel(CPlusPlusProject project) : base("Toolchain", project)
        {
            toolchains = new List<IToolchain>(IoC.GetInstances<IToolchain>());
            selectedToolchain = project.ToolChain;

            if(selectedToolchain != null)
            {
                ConfigPages = selectedToolchain.GetConfigurationPages(Model);
                SelectedConfigPage = null;
                SelectedConfigPage = ConfigPages?.FirstOrDefault();
            }
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

        public List<IToolchain> Toolchains
        {
            get { return toolchains; }
            set { this.RaiseAndSetIfChanged(ref toolchains, value); }
        }

        public IToolchain SelectedToolchain
        {
            get
            {
                return selectedToolchain;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedToolchain, value);

                Save();

                if (value != null)
                {
                    ConfigPages = value.GetConfigurationPages(Model);
                    SelectedConfigPage = null;
                    SelectedConfigPage = ConfigPages?.FirstOrDefault();
                }
            }
        }

        public void Save()
        {
            Model.ToolchainReference = selectedToolchain?.GetType().ToString();
            Model.Save();
        }
    }
}