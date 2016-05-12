using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvalonStudio.Toolchains.LocalGCC
{
    public class ToolchainSettingsViewModel : ViewModel<IProject>
    {
        public ToolchainSettingsViewModel(IProject model) : base(model)
        {
            CompileSettings = new CompileSettingsViewModel(model);
            LinkerSettings = new LinkSettingsFormViewModel(model);
        }

        private CompileSettingsViewModel compileSettings;
        public CompileSettingsViewModel CompileSettings
        {
            get { return compileSettings; }
            set { this.RaiseAndSetIfChanged(ref compileSettings, value); }
        }

        private LinkSettingsFormViewModel linkerSettings;
        public LinkSettingsFormViewModel LinkerSettings
        {
            get { return linkerSettings; }
            set { this.RaiseAndSetIfChanged(ref linkerSettings, value); }
        }
    }
}
