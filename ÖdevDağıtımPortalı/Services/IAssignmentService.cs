using ÖdevDağıtım.API.DTOs;

namespace ÖdevDağıtım.API.Services
{
    public interface IAssignmentService
    {
        Task<AssignmentReadDto> CreateAsync(AssignmentCreateDto dto);
        Task UpdateAsync(AssignmentUpdateDto dto);
        Task SoftDeleteAsync(int id);
        Task PublishAsync(int id);
        Task<IEnumerable<AssignmentReadDto>> GetAllAsync(int page, int pageSize);
        Task<AssignmentReadDto> GetByIdAsync(int id);
    }
}