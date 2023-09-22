﻿using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IPhotoService
	{
        string sasToken { get; }

        void GetBlobServiceClientSAS(ref BlobServiceClient blobServiceClient);

        Task<List<string>> ListContainers(BlobServiceClient blobServiceClient, string prefix, int? segmentSize);

        Task UploadFromFileAsync( IFormFile imageFile);

        Task DeleteBlob(BlobClient blob);

        Task<string> ConvertIFormFileToStringPathAsync(IFormFile file);

        bool photoExists(IFormFile photo);
    }
}

