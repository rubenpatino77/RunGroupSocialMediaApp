﻿using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using RunGroupSocialMedia.Helpers;
using RunGroupSocialMedia.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace RunGroupSocialMedia.Services
{
	public class PhotoService : IPhotoService
	{

        private readonly Cloudinary _cloundinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var acc = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloundinary = new Cloudinary(acc);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloundinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicUrl)
        {
            var publicId = publicUrl.Split('/').Last().Split('.')[0];
            var deleteParams = new DeletionParams(publicId);
            return await _cloundinary.DestroyAsync(deleteParams);
        }

        /*
        public string accountName = "rungroup";
        public string containerName = "run-group-container";
        public string containerUrl = "https://rungroup.blob.core.windows.net/run-group-container/";

        public string sasToken;

        string IPhotoService.sasToken => sasToken;
        string IPhotoService.containerUrl => containerUrl;

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


        public async Task UploadFromFileAsync( IFormFile imageFile)
        {
            string path = await ConvertIFormFileToStringPathAsync(imageFile);
            string fileName = imageFile.FileName;

            BlobServiceClient blobServiceClient = null; // Initialize as null
            GetBlobServiceClientSAS(ref blobServiceClient);

            BlobClient blobClient = blobServiceClient.GetBlobContainerClient("run-group-container").GetBlobClient(fileName);

            await blobClient.UploadAsync(path.ToString(), true);
        }

        public bool photoExists(IFormFile photo)
        {
            string fileName = photo.FileName;
            BlobServiceClient blobServiceClient = null; // Initialize as null
            GetBlobServiceClientSAS(ref blobServiceClient);
            BlobClient blobClient = blobServiceClient.GetBlobContainerClient("run-group-container").GetBlobClient(fileName);

            return blobClient.Exists();
        }

        public async Task DeleteBlobAsync(string fileName)
        {

            BlobServiceClient blobServiceClient = null; // Initialize as null
            GetBlobServiceClientSAS(ref blobServiceClient);

            BlobClient blobClient = blobServiceClient.GetBlobContainerClient("run-group-container").GetBlobClient(fileName);

            await blobClient.DeleteAsync();
        }*/
    }
}

