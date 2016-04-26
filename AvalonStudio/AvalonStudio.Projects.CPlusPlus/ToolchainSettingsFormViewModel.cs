namespace AvalonStudio.Projects.CPlusPlus
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using Perspex.Controls;
    using ReactiveUI;
    using Shell;
    using System.Collections.Generic;
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

        private Control toolchainSettingsControl;
        public Control ToolchainSettingsControl
        {
            get { return toolchainSettingsControl; }
            set { this.RaiseAndSetIfChanged(ref toolchainSettingsControl, value); }
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
                    ToolchainSettingsControl = value.GetSettingsControl(Model);
                }
            }
        }


    }
}
