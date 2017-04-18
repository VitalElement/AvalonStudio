using System;
using System.IO;
using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace AvalonStudio.Repositories
{
    public class PackageSourceOld
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public string CatalogGitDirectory => Path.Combine(CatalogDirectory, ".git");

        public string CatalogDirectory => Path.Combine(Platform.RepoCatalogDirectory, Name);

        private void Clone()
        {
            LibGit2Sharp.Repository.Clone(Url, CatalogDirectory);
        }

        public async Task<RepositoryOld> DownloadCatalog()
        {
            if (Directory.Exists(CatalogDirectory))
            {
                if (LibGit2Sharp.Repository.IsValid(CatalogGitDirectory))
                {
                    var repo = new LibGit2Sharp.Repository(CatalogGitDirectory);

                    await Task.Factory.StartNew(() => { LibGit2Sharp.Commands.Pull(repo, new Signature("AvalonStudio", "catalog@avalonstudio", new DateTimeOffset(DateTime.Now)), new PullOptions()); });
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

            var result = SerializedObject.Deserialize<RepositoryOld>(Path.Combine(CatalogDirectory, RepositoryOld.PackagesFileName));
            result.Source = this;

            foreach (var package in result.Packages)
            {
                package.Repository = result;
            }

            return result;
        }
    }
}