namespace AvalonStudio.Repositories
{
    using System.Collections.Generic;
    using AvalonStudio.Utils;
    using System.IO;

    public class PackageInfo
    {
        public List<string> Versions { get; set; }        
    }


    public class PackageReference
    {
        public PackageInfo DownloadInfo()
        {
            var source = Repository.Source;

            var packageInfoDir = Path.Combine(source.CatalogDirectory, Name);

            if (Directory.Exists(packageInfoDir))
            {
                Directory.Delete(packageInfoDir, true);
            }

            var repoPath = LibGit2Sharp.Repository.Clone(Url, packageInfoDir, new LibGit2Sharp.CloneOptions() { BranchName = "master", Checkout = false, IsBare = true });

            var repo = new LibGit2Sharp.Repository(repoPath);            

            var tags = repo.Tags;
            var branches = repo.Branches;
            
            // Platform == Branch
            // tag names are: [Platform].[Version]

            //

            var package = new PackageInfo();
            return package;
        }

        public Repository Repository { get; internal set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Repository : SerializedObject<Repository>
    {        
        public const string PackagesFileName = "Packages.json";        
         
        public Repository()
        {
            Packages = new List<PackageReference>();
        }

        public PackageSource Source { get; internal set; }
        public IList<PackageReference> Packages { get; set; }
    }
}
