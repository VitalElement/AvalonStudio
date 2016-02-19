namespace AvalonStudio.Repositories
{
    using System.Collections.Generic;
    using AvalonStudio.Utils;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    public class PackageInfo
    {
        public List<string> Versions { get; set; }        
    }


    public class PackageReference
    {
        public async Task<PackageIndex> DownloadInfoAsync()
        {
            var source = Repository.Source;

            PackageIndex result = null;

            using (var client = new WebClient())
            {
                var packageIndex = PackageIndex.FromString(await client.DownloadStringTaskAsync(Url));

                result = packageIndex;
            }

            return result;
        }

        public Repository Repository { get; internal set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Repository : SerializedObject<Repository>
    {        
        public const string PackagesFileName = "packages.json";        
         
        public Repository()
        {
            Packages = new List<PackageReference>();
        }

        public PackageSource Source { get; internal set; }
        public IList<PackageReference> Packages { get; set; }
    }
}
