using Microsoft.AspNetCore.Http;

namespace HomeworkPortal.API.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }
}