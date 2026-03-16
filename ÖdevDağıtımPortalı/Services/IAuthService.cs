using ÖdevDağıtım.API.DTOs;

namespace ÖdevDağıtım.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<bool> AssignRoleAsync(string email, string roleName);
    }
}