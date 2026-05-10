using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HomeworkPortal.API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
    }
}