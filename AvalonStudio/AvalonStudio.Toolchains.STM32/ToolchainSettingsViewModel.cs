using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvalonStudio.Toolchains.STM32
{
    public class ToolchainSettingsViewModel : ViewModel<IProject>
    {
        public ToolchainSettingsViewModel(IProject model) : base(model)
        {
            CompileSettings = new CompileSettingsFormViewModel(model);
            LinkerSettings = new LinkerSettingsFormViewModel(model);
        }

        private CompileSettingsFormViewModel compileSettings;
        public CompileSettingsFormViewModel CompileSettings
        {
            get { return compileSettings; }
            set { this.RaiseAndSetIfChanged(ref compileSettings, value); }
        }

        private LinkerSettingsFormViewModel linkerSettings;
        public LinkerSettingsFormViewModel LinkerSettings
        {
            get { return linkerSettings; }
            set { this.RaiseAndSetIfChanged(ref linkerSettings, value); }
        }
    }
}
