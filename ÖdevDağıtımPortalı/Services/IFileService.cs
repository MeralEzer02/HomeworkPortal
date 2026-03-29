using Microsoft.AspNetCore.Http;

namespace ÖdevDağıtım.API.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }
}