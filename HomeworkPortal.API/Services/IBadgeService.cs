namespace HomeworkPortal.API.Services
{
    public interface IBadgeService
    {
        Task CheckAndAwardBadgesAsync(string userId);
    }
}