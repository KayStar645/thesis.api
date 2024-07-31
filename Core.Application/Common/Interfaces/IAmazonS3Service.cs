using Microsoft.AspNetCore.Http;

namespace Core.Application.Common.Interfaces
{
    public interface IAmazonS3Service
    {
        Task<string> UploadFileAsync(string pPath, IFormFile pFile);

        Task<string> GetFileCidFromS3Async(string key);
    }
}
