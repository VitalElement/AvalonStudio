using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AvalonStudio.Models.PackageManager
{
    public class Repository
    {
        public const string CatalogUri = "https://github.com/danwalmsley/VERepos.git";

        public Repository()
        {
            this.Packages = new List<Package> ();
        }

        public static async Task<Repository> DownloadCatalog()
        {
            await Task.Factory.StartNew (() =>
            {
                if (!LibGit2Sharp.Repository.IsValid (Path.Combine (AvalonStudioService.RepoCatalogFolder, ".git")))
                {
                    string result = LibGit2Sharp.Repository.Clone (CatalogUri, AvalonStudioService.RepoCatalogFolder, new LibGit2Sharp.CloneOptions () { Checkout = true });
                }
                else
                {
                    var repo = new LibGit2Sharp.Repository (Path.Combine (AvalonStudioService.RepoCatalogFolder, ".git"));

                    var result = repo.Network.Pull (new LibGit2Sharp.Signature ("VEStudio", "catalog@vestudio", new DateTimeOffset (DateTime.Now)), new LibGit2Sharp.PullOptions () { });
                }
            });

            return Load (Path.Combine (AvalonStudioService.RepoCatalogFolder, "VERepo.xml"));
        }

        private static Repository Load (string location)
        {
            var serializer = new XmlSerializer (typeof (Repository));

            var textReader = new StreamReader (location);

            var result = (Repository)serializer.Deserialize (textReader);

            textReader.Close ();

            return result;
        }

        private static Package CatalogPackage(Package package)
        {
            Package result = new Package ();

            result.Name = package.Name;
            result.Description = package.Description;
            result.Version = package.Description;
            result.RepoUri = package.RepoUri;

            return result;
        }

        public static void GenerateCatalog ()
        {
            Repository repo = new Repository ();

            repo.Packages.Add (ClangToolChainPackage.GenerateClangPackage());

            repo.Packages.Add (GccToolChainPackage.GenerateGCCPackage ());

            repo.Packages.Add (BitThunderToolChainPackage.GeneratePackage ());

            repo.Packages.Add(MinGWToolChainPackage.GeneratePackage());
            

            repo.SerializeToXml (@"c:\VESTudio\VERepo.xml");
        }


        public static async void TestPackager ()
        {
            GenerateCatalog ();

            //ToolChainPackage.GeneratePackage ();

            var repo = await Repository.DownloadCatalog ();

            var package = await repo.Packages [0].DownloadPackage (null);

            Console.WriteLine(package.Name);
            package.Install ();
        }

        public List<Package> Packages { get; set; }

        public void SerializeToXml (string location)
        {
            try
            {
                var serializer = new XmlSerializer (typeof (Repository));

                var textWriter = new StreamWriter (location);

                serializer.Serialize (textWriter, this);

                textWriter.Close ();
            }
            catch (Exception e)
            {
                Console.WriteLine (e);
            }
        }
    }
}
