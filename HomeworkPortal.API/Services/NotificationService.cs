using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using HomeworkPortal.API.DTOs;
using HomeworkPortal.API.Models;
using HomeworkPortal.API.Repositories;
using HomeworkPortal.API.Helpers;
using Microsoft.AspNetCore.SignalR;
using HomeworkPortal.API.Hubs;

namespace HomeworkPortal.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<NotificationService> logger,
            IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task CreateNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false
            };

            var retryPolicy = RetryHelper.CreateRetryPolicy(_logger);

            await retryPolicy.ExecuteAsync(async () =>
            {
                await _unitOfWork.Notifications.AddAsync(notification);
                await _unitOfWork.CompleteAsync();
            });

            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task CreateNotificationsAsync(IEnumerable<string> userIds, string message)
        {
            var userIdList = userIds.ToList();
            if (!userIdList.Any()) return;

            var notifications = userIdList.Select(userId => new Notification
            {
                UserId = userId,
                Message = message,
                IsRead = false
            }).ToList();

            const int chunkSize = 100;
            var retryPolicy = RetryHelper.CreateRetryPolicy(_logger);

            for (int i = 0; i < notifications.Count; i += chunkSize)
            {
                var chunk = notifications.Skip(i).Take(chunkSize);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    foreach (var notif in chunk)
                    {
                        await _unitOfWork.Notifications.AddAsync(notif);
                    }
                    await _unitOfWork.CompleteAsync();
                });

                foreach (var notif in chunk)
                {
                    await _hubContext.Clients.User(notif.UserId).SendAsync("ReceiveNotification", notif.Message);
                }
            }
        }

        public async Task<PagedResult<NotificationReadDto>> GetUserNotificationsAsync(string userId, PaginationParams paginationParams)
        {
            var query = _unitOfWork.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted)
                .OrderByDescending(n => n.Created);

            var totalCount = await query.CountAsync();

            var notifications = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            var dtoList = _mapper.Map<IEnumerable<NotificationReadDto>>(notifications);
            return new PagedResult<NotificationReadDto>(dtoList, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _unitOfWork.Notifications.GetByIdAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}