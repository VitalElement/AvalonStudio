
using NuGet.Common;
using NuGet.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AvalonStudio.Packages
{
    public class MachineWideSettings : IMachineWideSettings
    {
        private readonly Lazy<IEnumerable<Settings>> _settings;

        public MachineWideSettings()
        {
            var baseDirectory = NuGetEnvironment.GetFolderPath(NuGetFolderPath.MachineWideConfigDirectory);
            _settings = new Lazy<IEnumerable<Settings>>(() => NuGet.Configuration.Settings.LoadMachineWideSettings(baseDirectory).Cast<Settings>());
        }

        public IEnumerable<Settings> Settings => _settings.Value;
    }
}
