using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using RunGroupSocialMedia.Helpers;
using RunGroupSocialMedia.Interfaces;

namespace RunGroupSocialMedia.Services
{
	public class PhotoService : IPhotoService
	{
        public string accountName = "rungroup";
        public string containerName = "run-group-container";

        public string sasToken;

        string IPhotoService.sasToken => sasToken;

        public PhotoService(IOptions<AzureBlobStorageSettings> options)
		{
            sasToken = options.Value.SasToken;
		}

        public void GetBlobServiceClientSAS(ref BlobServiceClient blobServiceClient)
        {
            string blobUri = "https://" + accountName + ".blob.core.windows.net";

            blobServiceClient = new BlobServiceClient
            (new Uri($"{blobUri}?{sasToken}"), null);
        }

        public async Task<List<string>> ListContainers(BlobServiceClient blobServiceClient, string prefix, int? segmentSize)
        {
            try
            {
                var containerNames = new List<string>();

                // Call the listing operation and enumerate the result segment.
                var resultSegment =
                    blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, default)
                    .AsPages(default, segmentSize);

                await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
                {
                    foreach (BlobContainerItem containerItem in containerPage.Values)
                    {
                        containerNames.Add(containerItem.Name);
                    }
                }

                return containerNames;
            }
            catch (RequestFailedException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<string> ConvertIFormFileToStringPathAsync(IFormFile file)
        {
            if (file != null)
            {
                // Define the directory where you want to save the file
                var uploadDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                // Create the directory if it doesn't exist
                Directory.CreateDirectory(uploadDirectory);

                // Get a unique file name
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // Combine the directory and file name to get the full file path
                var filePath = Path.Combine(uploadDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return the file path as a string
                return filePath;
            }

            // Return null or throw an exception if 'file' is null
            return null;
        }


        public async Task UploadFromFileAsync( BlobContainerClient containerClient, string localFilePath, string fileName)
        {
            //string fileName = Path.GetFileName(localFilePath.ToString());
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(localFilePath.ToString(), true);
        }

        public async Task DeleteBlob(BlobClient blob)
        {
            await blob.DeleteAsync();
        }
    }
}

