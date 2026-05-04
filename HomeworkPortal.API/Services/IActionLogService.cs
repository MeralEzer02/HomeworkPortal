using HomeworkPortal.API.DTOs;

namespace HomeworkPortal.API.Services
{
    public interface IActionLogService
    {
        Task LogActionAsync(string? userId, string actionType, string description, string? entityName = null, int? entityId = null);
        Task<PagedResult<ActionLogDto>> GetLogsAsync(PaginationParams paginationParams);
    }
}