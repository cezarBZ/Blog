using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Blog.Infrastructure.Services.BlobStorage
{
    public class BlobStorageService : IFileStorageService
    {
        private readonly AzureBlobStorageOptions _options;
        public BlobStorageService(IOptions<AzureBlobStorageOptions> options)
        {
            _options = options.Value;
        }


        public async Task<string> UploadFileAsync(IFormFile file, string fileName)
        {
            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_options.ContainerName);

            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(fileName);
            await using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }


    }
}
