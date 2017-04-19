using AvalonStudio.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace AvalonStudio.Repositories
{
    public class PackageReference
    {
        public RepositoryOld Repository { get; internal set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public async Task<PackageIndex> DownloadInfoAsync()
        {
            var source = Repository.Source;

            PackageIndex result = null;

            using (var client = new HttpClient())
            {
                var packageIndex = SerializedObject.FromString<PackageIndex>(await client.GetStringAsync(Url));

                result = packageIndex;
            }

            return result;
        }
    }
}