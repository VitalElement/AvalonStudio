using System;
using System.Collections.Generic;
using System.IO;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using LibGit2Sharp;

namespace AvalonStudio.Repositories
{
	public class PackageSource
	{
		public string Name { get; set; }
		public string Url { get; set; }

		public string CatalogGitDirectory => Path.Combine(CatalogDirectory, ".git");

		public string CatalogDirectory => Path.Combine(Platform.RepoCatalogDirectory, Name);

		private void Clone()
		{
			LibGit2Sharp.Repository.Clone(Url, CatalogDirectory);
		}

		public Repository DownloadCatalog()
		{
			if (Directory.Exists(CatalogDirectory))
			{
				if (LibGit2Sharp.Repository.IsValid(CatalogGitDirectory))
				{
					var repo = new LibGit2Sharp.Repository(CatalogGitDirectory);

					repo.Network.Pull(new Signature("AvalonStudio", "catalog@avalonstudio", new DateTimeOffset(DateTime.Now)),
						new PullOptions());
				}
				else
				{
					Directory.Delete(CatalogDirectory, true);
					Clone();
				}
			}
			else
			{
				Clone();
			}

			var result = SerializedObject.Deserialize<Repository>(Path.Combine(CatalogDirectory, Repository.PackagesFileName));
			result.Source = this;

			foreach (var package in result.Packages)
			{
				package.Repository = result;
			}

			return result;
		}
	}

	public class PackageSources
	{
		private PackageSources()
		{
			Sources = new List<PackageSource>();
		}

		public static PackageSources Instance { get; private set; }

		public IList<PackageSource> Sources { get; set; }

		public static void InitialisePackageSources()
		{
			if (!File.Exists(Platform.PackageSourcesFile))
			{
				var sources = new PackageSources();

				sources.Sources.Add(new PackageSource
				{
					Name = "VitalElement",
					Url = "https://github.com/VitalElement/AvalonStudio.Repository"
				});

                SerializedObject.Serialize(Platform.PackageSourcesFile, sources);				
			}

			var result = SerializedObject.Deserialize<PackageSources>(Platform.PackageSourcesFile);

			Instance = result;
		}
	}
}