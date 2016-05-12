namespace AvalonStudio.Extensibility.Git
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LibGit2Sharp;
    using AvalonStudio.Utils;
    using LibGit2Sharp.Handlers;

    public class Git
    {
        public static async Task ClonePublicHttpSubmodule (IConsole console, string sourceurl, string workingPath)
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
                Repository.Clone(sourceurl, workingPath, new CloneOptions() { OnTransferProgress = transferHandler, OnCheckoutProgress = checkoutHandler });
            });
        }
    }
}
