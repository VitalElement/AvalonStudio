using AvalonStudio.GlobalSettings;
using AvalonStudio.Platforms;
using Newtonsoft.Json;
using System.IO;

namespace AvalonStudio.Toolchains.CustomGCC
{
    public class CustomGCCToolchainProjectSettings
    {
        public string InstanceName { get; set; } = "";

        public string Prefix { get; set; } = "";

        public string CCPrefix { get; set; } = "";

        public string CCName { get; set; } = "gcc";

        public string CPPPrefix { get; set; } = "";

        public string CPPName { get; set; } = "g++";

        public string LDPrefix { get; set; } = "";

        public string LDName { get; set; } = "ld";

        public string ARPrefix { get; set; } = "";

        public string ARName { get; set; } = "ar";

        public string SizePrefix { get; set; } = "";

        public string SizeName { get; set; } = "size";

        public string GDBPrefix { get; set; } = "";

        public string GDBName { get; set; } = "gdb";
        
        public string ExecutableExtension { get; set; } = "";

        public string StaticLibraryExtension { get; set; } = ".a";

        public string LibraryQueryCommand { get; set; } = "gcc";

        [JsonIgnore]
        public string[] ExtraPaths
        {
            get
            {
                var settings = CustomGCCToolchainProfiles.Instance;

                if (settings.Profiles.ContainsKey(InstanceName))
                {
                    return settings.Profiles[InstanceName].ExtraPaths;
                }
                else
                {
                    return new string[0];
                }
            }

            set
            {

            }
        }

        [JsonIgnore]
        public string BasePath
        {
            get
            {
                var settings = CustomGCCToolchainProfiles.Instance;

                if(settings.Profiles.ContainsKey(InstanceName))
                {
                    return settings.Profiles[InstanceName].BasePath;
                }
                else
                {
                    return "";
                }
            }
            set { }
        }

        private string ResolveCommand(string prefix, string name)
        {
            var settings = CustomGCCToolchainProfiles.Instance;

            string basePath = "";

            if (settings.Profiles.ContainsKey(InstanceName))
            {
                basePath = settings.Profiles[InstanceName].BasePath;
            }

            if (!string.IsNullOrEmpty(prefix))
            {
                if (prefix == "*")
                {
                    return Path.Combine(basePath, name) + Platform.ExecutableExtension;
                }
                else
                {
                    return Path.Combine(basePath, prefix + name) + Platform.ExecutableExtension;
                }
            }
            else
            {
                return Path.Combine(basePath, Prefix + name) + Platform.ExecutableExtension;
            }
        }

        [JsonIgnore]
        public string CCExecutable => ResolveCommand(CCPrefix, CCName);

        [JsonIgnore]
        public string CPPExecutable => ResolveCommand(CPPPrefix, CPPName);

        [JsonIgnore]
        public string LDExecutable => ResolveCommand(LDPrefix, LDName);

        [JsonIgnore]
        public string ARExecutable => ResolveCommand(ARPrefix, ARName);

        [JsonIgnore]
        public string SizeExecutable => ResolveCommand(SizePrefix, SizeName);

        [JsonIgnore]
        public string GDBExecutable => ResolveCommand(GDBPrefix, GDBName);
    }
}
 