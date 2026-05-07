using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeworkPortal.API.Data;
using System.Security.Claims;

namespace HomeworkPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("badges")]
        public async Task<IActionResult> GetMyBadges()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var myBadges = await _context.UserBadges
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId)
                .OrderByDescending(ub => ub.EarnedAt)
                .Select(ub => new
                {
                    ub.Badge.Name,
                    ub.Badge.Description,
                    ub.Badge.Icon,
                    EarnedAt = ub.EarnedAt
                })
                .ToListAsync();

            return Ok(myBadges);
        }

        [HttpGet("all-badges")]
        public async Task<IActionResult> GetAllSystemBadges()
        {
            var badges = await _context.Badges
                .OrderBy(b => b.RequiredCount)
                .Select(b => new
                {
                    b.Name,
                    b.Description,
                    b.Icon,
                    b.RequiredCount
                })
                .ToListAsync();

            return Ok(badges);
        }
    }
}