using AvalonStudio.Packaging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Core.Util;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 6)
            {
                Console.WriteLine("Usage: filename - toolchain name - platform - version - type - access token");
                Console.WriteLine("i.e. : clang8.7z - clang8 -win-x64 - 8.0.0 - package - connectionString");
                Console.WriteLine("i.e. : arm-none-eabi - armgcc - linux-x64 - 7.2.0 - toolchainconfig - connectionString");
                Console.WriteLine("i.e. : arm-none-eabi.config.7z - any - 7.2.0 - toolchainconfig - connectionString");
                return;
            }

            var toolchains = await PackageManager.ListToolchains();

            foreach(var tc in toolchains)
            {
                Console.WriteLine(tc);

                var packages =  await PackageManager.ListToolchainPackages(tc, true, true);

                if (packages != null)
                {
                    foreach (var package in packages)
                    {
                        Console.WriteLine($"   {package.Name} - {package.Platform} - {package.Version} - {package.Published} - {package.Size / 1024}");
                    }
                }
            }

            var fileName = args[0];
            var toolchainName = args[1];
            var platform = args[2];
            var version = args[3];
            var type = args[4];
            var storageConnectionString = args[5];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out var storageAccount))
            {
                try
                {
                    // Create the CloudBlobClient that represents the Blob storage endpoint for the storage account.
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    var containers = await cloudBlobClient.ListContainersSegmentedAsync(new BlobContinuationToken());

                    foreach (var container in containers.Results)
                    {
                        Console.WriteLine(container.Name);
                    }


                    var cloudBlobContainer = cloudBlobClient.GetContainerReference(toolchainName.Replace(".", "-").ToLower());
                    cloudBlobContainer.Metadata["type"] = type;
                    

                    await cloudBlobContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, default(BlobRequestOptions), default(OperationContext));


                    // Get a reference to the blob address, then upload the file to the blob.
                    // Use the value of localFileName for the blob name.
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(fileName));
                    var fileInfo = new FileInfo(fileName);

                    var progress = new Progress<StorageProgress>(p =>
                    {
                        Console.WriteLine($"Uploaded: {p.BytesTransferred} / {fileInfo.Length}");
                    });


                    cloudBlockBlob.Metadata["platform"] = platform;
                    cloudBlockBlob.Metadata["version"] = version;

                    await cloudBlockBlob.UploadFromFileAsync(fileName, default(AccessCondition), default(BlobRequestOptions), default(OperationContext), progress, new System.Threading.CancellationToken());
                    // List the blobs in the container.

                    Console.WriteLine("Listing blobs in container.");

                    BlobContinuationToken blobContinuationToken = null;
                    do
                    {
                        var resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(null, blobContinuationToken);
                        // Get the value of the continuation token returned by the listing call.
                        blobContinuationToken = resultSegment.ContinuationToken;
                        foreach (IListBlobItem item in resultSegment.Results)
                        {
                            Console.WriteLine(item.Uri);
                        }
                    } while (blobContinuationToken != null); // Loop while the continuation token is not null.
                    Console.WriteLine();

                    // Download the blob to a local file, using the reference created earlier. 
                    // Append the string "_DOWNLOADED" before the .txt extension so that you can see both files in MyDocuments.
                    var destinationFile = fileName.Replace("7z", "_DOWNLOADED.7z");
                    Console.WriteLine("Downloading blob to {0}", destinationFile);
                    Console.WriteLine();
                    await cloudBlockBlob.DownloadToFileAsync(destinationFile, FileMode.Create, null, new BlobRequestOptions(), null, new Progress<StorageProgress>(p =>
                    {
                        Console.WriteLine($"Downloaded: {p.BytesTransferred} / {cloudBlockBlob.Properties.Length}");

                    }), new System.Threading.CancellationToken());

                    Console.WriteLine(cloudBlockBlob.Uri);
                }
                catch (StorageException ex)
                {
                    Console.WriteLine("Error returned from the service: {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named 'storageconnectionstring' with your storage " +
                    "connection string as a value.");
            }
        }
    }
}
