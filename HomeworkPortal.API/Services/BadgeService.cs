using HomeworkPortal.API.Data;
using HomeworkPortal.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeworkPortal.API.Services
{
    public class BadgeService : IBadgeService
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;

        public BadgeService(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task CheckAndAwardBadgesAsync(string userId)
        {
            int submissionCount = await _context.Submissions
                .CountAsync(s => s.StudentId == userId && !s.IsDeleted);

            var earnedBadgeIds = await _context.UserBadges
                .Where(ub => ub.UserId == userId)
                .Select(ub => ub.BadgeId)
                .ToListAsync();

            var badgesToAward = await _context.Badges
                .Where(b => b.ConditionType == "SUBMISSION_COUNT"
                         && b.RequiredCount <= submissionCount
                         && !earnedBadgeIds.Contains(b.Id))
                .ToListAsync();

            if (badgesToAward.Any())
            {
                foreach (var badge in badgesToAward)
                {
                    _context.UserBadges.Add(new UserBadge
                    {
                        UserId = userId,
                        BadgeId = badge.Id
                    });

                    await _notificationService.CreateNotificationAsync(
                        userId,
                        $"🏆 TEBRİKLER! Yeni bir rozet kazandın: {badge.Name}. {badge.Description}"
                    );
                }

                await _context.SaveChangesAsync();
            }
        }
    }
}