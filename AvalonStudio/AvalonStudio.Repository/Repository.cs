using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AvalonStudio.Utils;

namespace AvalonStudio.Repositories
{
	public class PackageInfo
	{
		public List<string> Versions { get; set; }
	}


	public class PackageReference
	{
		public Repository Repository { get; internal set; }
		public string Name { get; set; }
		public string Url { get; set; }

		public async Task<PackageIndex> DownloadInfoAsync()
		{
			var source = Repository.Source;

			PackageIndex result = null;

			using (var client = new WebClient())
			{
				var packageIndex = SerializedObject.FromString<PackageIndex>(await client.DownloadStringTaskAsync(Url));

				result = packageIndex;
			}

			return result;
		}
	}

	public class Repository
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