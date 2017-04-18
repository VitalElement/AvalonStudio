namespace AvalonStudio.Repositories
{
    using AvalonStudio.Platforms;
    using AvalonStudio.Utils;
    using LibGit2Sharp;
    using LibGit2Sharp.Handlers;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    public class PackageIndex
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
            TransferProgressHandler transferHandler = e =>
            {
                //Console.WriteLine (string.Format ("Bytes: {1}, Objects: {2}/{3}, Indexed: {0}", e.IndexedObjects, ByteSizeHelper.ToString (e.ReceivedBytes),
                //	e.ReceivedObjects, e.TotalObjects));
                return true;
            };

            CheckoutProgressHandler checkoutHandler =
                (path, steps, totalSteps) =>
                {
                    console.OverWrite($"Checkout: {steps}/{totalSteps}");
                };

            var repo = Path.Combine(Platform.ReposDirectory, Name);
            LibGit2Sharp.Repository repository = null;

            if (LibGit2Sharp.Repository.IsValid(repo))
            {
                repository = new LibGit2Sharp.Repository(repo);

                await Task.Factory.StartNew(() =>
                {
                    LibGit2Sharp.Commands.Fetch(repository, repository.Network.Remotes["origin"].Name, repository.Network.Remotes["origin"].FetchRefSpecs.Select(s => s.Specification),
                        new FetchOptions { OnTransferProgress = transferHandler, TagFetchMode = TagFetchMode.All }, "");
                });
            }
            else
            {
                if (Directory.Exists(repo))
                {
                    Directory.Delete(repo, true);
                }

                await Task.Factory.StartNew(() =>
                {
                    LibGit2Sharp.Repository.Clone(RepoUrl, repo,
                        new CloneOptions
                        {
                            OnTransferProgress = transferHandler,
                            Checkout = false,
                            OnCheckoutProgress = checkoutHandler
                        });
                    repository = new LibGit2Sharp.Repository(repo);
                });
            }

            await Task.Factory.StartNew(() =>
            {
                LibGit2Sharp.Commands.Checkout(repository, tag, new CheckoutOptions { OnCheckoutProgress = checkoutHandler });
            });

            console.OverWrite("Package installed Successfully.");
        }
    }
}