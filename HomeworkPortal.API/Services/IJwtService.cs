using HomeworkPortal.API.Models;

namespace HomeworkPortal.API.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}