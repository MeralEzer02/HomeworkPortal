using ÖdevDağıtım.API.Models;

namespace ÖdevDağıtım.API.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}