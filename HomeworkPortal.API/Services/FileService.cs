using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace HomeworkPortal.API.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".pdf", ".doc", ".docx", ".zip", ".rar" };
        private readonly int _maxFileSize = 5 * 1024 * 1024; // 5 MB limit

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new Exception("Yüklenecek dosya bulunamadı veya dosya boş.");

            if (file.Length > _maxFileSize)
                throw new Exception($"Dosya boyutu çok büyük. Maksimum sınır: 5MB.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
                throw new Exception($"Geçersiz dosya tipi. İzin verilen uzantılar: {string.Join(", ", _allowedExtensions)}");

            var fileName = Guid.NewGuid().ToString() + extension;

            var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", folderName);

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", folderName, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            var fullPath = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}