namespace AvalonStudio.Repositories
{
    using AvalonStudio.Utils;
    using Platform;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class PackageSource
    {
        private void Clone()
        {
            LibGit2Sharp.Repository.Clone(Url, CatalogDirectory);
        }        

        public Repository DownloadCatalog ()
        {
            if(Directory.Exists(CatalogDirectory))
            {
                if(LibGit2Sharp.Repository.IsValid(CatalogGitDirectory))
                {
                    var repo = new LibGit2Sharp.Repository(CatalogGitDirectory);

                    repo.Network.Pull(new LibGit2Sharp.Signature("AvalonStudio", "catalog@avalonstudio", new DateTimeOffset(DateTime.Now)), new LibGit2Sharp.PullOptions());
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
            
            var result = Repository.Deserialize(Path.Combine(CatalogDirectory, Repository.PackagesFileName));
            result.Source = this;
            
            foreach(var package in result.Packages)
            {
                package.Repository = result;
            }

            return result;
        }

        public string Name { get; set; }
        public string Url { get; set; }

        public string CatalogGitDirectory
        {
            get
            {
                return Path.Combine(CatalogDirectory, ".git");
            }
        }

        public string CatalogDirectory
        {
            get
            {
                return Path.Combine(Platform.RepoCatalogDirectory, Name);
            }
        }


    }

    public class PackageSources : SerializedObject<PackageSources>
    {
        private PackageSources()
        {
            Sources = new List<PackageSource>();
        }

        public static PackageSources Instance { get; private set; }

        public static void InitialisePackageSources()
        {
            if (!File.Exists(Platform.PackageSourcesFile))
            {
                var sources = new PackageSources();
                
                sources.Sources.Add(new PackageSource() { Name= "VitalElement", Url = "https://github.com/VitalElement/AvalonStudio.Repository" });

                sources.Serialize(Platform.PackageSourcesFile);
            }

            var result = PackageSources.Deserialize(Platform.PackageSourcesFile);

            Instance = result;
        }
        
        public IList<PackageSource> Sources { get; set; }
    }
}
