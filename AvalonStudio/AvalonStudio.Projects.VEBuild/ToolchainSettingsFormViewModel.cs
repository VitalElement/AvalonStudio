namespace AvalonStudio.Projects.VEBuild
{
    using AvalonStudio.MVVM;
    using ReactiveUI;
    using System.Collections.Generic;

    public class ToolchainSettingsFormViewModel : ViewModel
    {
        public ToolchainSettingsFormViewModel()
        {
            toolchains = new List<string>();
            toolchains.Add("STM32");
        }

        private List<string> toolchains;
        public List<string> Toolchains
        {
            get { return toolchains; }
            set { this.RaiseAndSetIfChanged(ref toolchains, value); }
        }

    }
}
