using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Toolchains.CustomGCC
{
    class CustomGCCToolchainProfile
    {
        public string BasePath { get; set; } = "";
        public string[] ExtraPaths { get; set; } = new string[0];
    }

    class CustomGCCToolchainProfiles : IActivatableExtension
    {
        private static string ProfilesFile = Path.Combine(Platform.SettingsDirectory, "GccProfiles.json");

        public Dictionary<string, CustomGCCToolchainProfile> Profiles { get; } = new Dictionary<string, CustomGCCToolchainProfile>();

        public static CustomGCCToolchainProfiles Instance { get; private set; }

        public void Save()
        {
            SerializedObject.Serialize(ProfilesFile, this);
        }

        private static CustomGCCToolchainProfiles Load()
        {
            if(!File.Exists(ProfilesFile))
            {
                SerializedObject.Serialize(ProfilesFile, new CustomGCCToolchainProfiles());
            }

            return SerializedObject.Deserialize<CustomGCCToolchainProfiles>(ProfilesFile);
        }

        public void BeforeActivation()
        {
            
        }

        public void Activation()
        {
            Instance = Load();
        }
    }
}
