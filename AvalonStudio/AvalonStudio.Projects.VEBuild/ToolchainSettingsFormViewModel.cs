namespace AvalonStudio.Projects.VEBuild
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using ReactiveUI;
    using System.Collections.Generic;
    using Toolchains;

    public class ToolchainSettingsFormViewModel : ViewModel<CPlusPlusProject>
    {
        public ToolchainSettingsFormViewModel(CPlusPlusProject project) : base (project)
        {
            toolchains = new List<IToolChain>(Workspace.Instance.ToolChains);
            selectedToolchain = project.ToolChain;
        }

        public void Save ()
        {
            Model.ToolchainReference = selectedToolchain?.GetType().ToString();
            Model.Save();
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
            set { this.RaiseAndSetIfChanged(ref selectedToolchain, value); Save(); }
        }


    }
}
