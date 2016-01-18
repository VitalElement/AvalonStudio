namespace AvalonStudio.Projects.VEBuild
{
    using AvalonStudio.MVVM;
    using Extensibility;
    using ReactiveUI;
    using System.Collections.Generic;

    public class ToolchainSettingsFormViewModel : ViewModel<VEBuildProject>
    {
        public ToolchainSettingsFormViewModel(VEBuildProject project) : base (project)
        {
            toolchains = new List<string>();
            
            foreach(var toolchain in Workspace.Instance.ToolChains)
            {
                toolchains.Add(toolchain.GetType().ToString());
            }
        }

        public void Save ()
        {
            Model.ToolchainReference = selectedToolchain;
            Model.Save();
        }

        private List<string> toolchains;
        public List<string> Toolchains
        {
            get { return toolchains; }
            set { this.RaiseAndSetIfChanged(ref toolchains, value); }
        }

        private string selectedToolchain;
        public string SelectedToolchain
        {
            get { return selectedToolchain; }
            set { this.RaiseAndSetIfChanged(ref selectedToolchain, value); }
        }


    }
}
