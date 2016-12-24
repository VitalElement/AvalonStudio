using System.Threading.Tasks;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Utils;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace AvalonStudio.Extensibility.Git
{
	public class Git
	{
		public static async Task ClonePublicHttpSubmodule(IConsole console, string sourceurl, string workingPath)
		{
			TransferProgressHandler transferHandler = e =>
			{
				console.OverWrite(
					$"Bytes: {ByteSizeHelper.ToString(e.ReceivedBytes)}, Objects: {e.ReceivedObjects}/{e.TotalObjects}, Indexed: {e.IndexedObjects}");
				return true;
			};

			CheckoutProgressHandler checkoutHandler =
				(path, steps, totalSteps) => { console.OverWrite($"Checkout: {steps}/{totalSteps}"); };

			await
				Task.Factory.StartNew(
					() =>
					{
						Repository.Clone(sourceurl, workingPath,
							new CloneOptions {OnTransferProgress = transferHandler, OnCheckoutProgress = checkoutHandler});
					});
		}
	}
}