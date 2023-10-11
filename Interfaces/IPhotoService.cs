using System;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudinaryDotNet.Actions;

namespace RunGroupSocialMedia.Interfaces
{
	public interface IPhotoService
	{
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);

        Task<DeletionResult> DeletePhotoAsync(string publicUrl);

        /* BELOW WORKS FOR AZURE
        
        string sasToken { get; }
        string containerUrl { get; }

        void GetBlobServiceClientSAS(ref BlobServiceClient blobServiceClient);

        Task<List<string>> ListContainers(BlobServiceClient blobServiceClient, string prefix, int? segmentSize);

        Task UploadFromFileAsync( IFormFile imageFile);

        Task<string> ConvertIFormFileToStringPathAsync(IFormFile file);

        bool photoExists(IFormFile photo);

        Task DeleteBlobAsync(string fileName);*/
    }
}

