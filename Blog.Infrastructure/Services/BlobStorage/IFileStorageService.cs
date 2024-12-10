using Microsoft.AspNetCore.Http;

namespace Blog.Infrastructure.Services.BlobStorage
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(IFormFile fileStream, string fileName);
    }
}
