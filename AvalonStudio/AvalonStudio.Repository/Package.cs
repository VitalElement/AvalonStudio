namespace AvalonStudio.Repositories
{
    using AvalonStudio.Utils;
    using Platform;
    using LibGit2Sharp.Handlers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    public class Package : SerializedObject<Package>
    {
        public Package()
        {

        }

        //public Repository Repository { get; internal set; }
        public const string PackageFile = "package.json";
        public string Plugin { get; set; }

    }

    public class PackageIndex : SerializedObject<PackageIndex>
    {
        public PackageIndex()
        {
            Tags = new List<string>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string RepoUrl { get; set; }
        public List<string> Tags { get; set; }

        public async Task Synchronize(string tag, IConsole console)
        {
            TransferProgressHandler transferHandler = (e) =>
            {
                console.OverWrite(string.Format("Bytes: {1}, Objects: {2}/{3}, Indexed: {0}", e.IndexedObjects, e.ReceivedBytes, e.ReceivedObjects, e.TotalObjects));
                return true;
            };

            CheckoutProgressHandler checkoutHandler = (path, steps, totalSteps) =>
            {
                console.OverWrite(string.Format("Checkout: {0}/{1}", steps, totalSteps));

            };

            await Task.Factory.StartNew(() =>
            {
                try {
                    string repo = Path.Combine(Platform.ReposDirectory, Name);
                    LibGit2Sharp.Repository repository = null;

                    if (LibGit2Sharp.Repository.IsValid(repo))
                    {
                        repository = new LibGit2Sharp.Repository(repo);
                        repository.Network.Fetch(repository.Network.Remotes["origin"], new LibGit2Sharp.FetchOptions() { OnTransferProgress = transferHandler, TagFetchMode = LibGit2Sharp.TagFetchMode.All });

                    }
                    else
                    {
                        if (Directory.Exists(repo))
                        {
                            Directory.Delete(repo, true);
                        }

                        LibGit2Sharp.Repository.Clone(RepoUrl, repo, new LibGit2Sharp.CloneOptions() { OnTransferProgress = transferHandler, Checkout = false, OnCheckoutProgress = checkoutHandler });
                        repository = new LibGit2Sharp.Repository(repo);
                    }

                    repository.Checkout(tag, new LibGit2Sharp.CheckoutOptions() { OnCheckoutProgress = checkoutHandler });

                    console.OverWrite("Package installed Successfully.");
                }
                catch (Exception e)
                {
                    console.OverWrite("Error installing package: " + e.Message);
                }                
            });
        }
    }
}
