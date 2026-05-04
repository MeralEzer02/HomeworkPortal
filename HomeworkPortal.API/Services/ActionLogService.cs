using HomeworkPortal.API.Data;
using HomeworkPortal.API.DTOs;
using HomeworkPortal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeworkPortal.API.Services
{
    public class ActionLogService : IActionLogService
    {
        private readonly AppDbContext _context;

        public ActionLogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task LogActionAsync(string? userId, string actionType, string description, string? entityName = null, int? entityId = null)
        {
            var log = new ActionLog
            {
                UserId = userId,
                ActionType = actionType,
                Description = description,
                EntityName = entityName,
                EntityId = entityId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.ActionLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ActionLogDto>> GetLogsAsync(PaginationParams paginationParams)
        {
            var query = _context.ActionLogs.Include(a => a.User).AsQueryable();

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .Select(a => new ActionLogDto
                {
                    Id = a.Id,
                    UserFullName = a.User != null ? a.User.FirstName + " " + a.User.LastName : "Sistem İşlemi",
                    ActionType = a.ActionType,
                    Description = a.Description,
                    EntityName = a.EntityName,
                    CreatedAt = a.CreatedAt
                })
                .ToListAsync();

            return new PagedResult<ActionLogDto>(logs, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }
    }
}