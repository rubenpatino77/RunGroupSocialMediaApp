using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using RunGroupSocialMedia.Interfaces;

namespace RunGroupSocialMedia.Services
{
	public class PhotoService : IPhotoService
	{
        string accountName = "rungroup";
        string sasToken = "sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2023-12-13T03:00:50Z&st=2023-09-12T18:00:50Z&spr=https,http&sig=%2Fn%2FsoMSl7%2Bj01vuytU1QgGjueBTK7dt7yNW4PLPNIXU%3D";

        public PhotoService()
		{
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

        public async Task UploadFromFileAsync( BlobContainerClient containerClient, string localFilePath)
        {
            string fileName = Path.GetFileName(localFilePath);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(localFilePath, true);
        }

        public async Task DeleteBlob(BlobClient blob)
        {
            await blob.DeleteAsync();
        }
    }
}

