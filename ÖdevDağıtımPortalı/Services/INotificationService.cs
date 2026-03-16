using ÖdevDağıtım.API.DTOs;

namespace ÖdevDağıtım.API.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationReadDto>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int id);
    }
}