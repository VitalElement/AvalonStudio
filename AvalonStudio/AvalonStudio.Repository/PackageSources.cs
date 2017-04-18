using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Repositories
{
    public class PackageSources
    {
        private PackageSources()
        {
            Sources = new List<PackageSourceOld>();
        }

        public static PackageSources Instance { get; private set; }

        public IList<PackageSourceOld> Sources { get; set; }

        public static void InitialisePackageSources()
        {
            PackageSources result = null;

            if (!File.Exists(Platform.PackageSourcesFile) || (result = SerializedObject.Deserialize<PackageSources>(Platform.PackageSourcesFile)) == null)
            {
                var sources = new PackageSources();

                sources.Sources.Add(new PackageSourceOld
                {
                    Name = "VitalElement",
                    Url = "https://github.com/VitalElement/AvalonStudio.Repository"
                });

                SerializedObject.Serialize(Platform.PackageSourcesFile, sources);

                result = sources;
            }

            Instance = result;
        }
    }
}