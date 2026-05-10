using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeworkPortal.API.Models;
using HomeworkPortal.API.Data;
using HomeworkPortal.API.DTOs;

namespace HomeworkPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public DashboardController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var students = await _userManager.GetUsersInRoleAsync("Student");
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var now = DateTime.UtcNow;
            var fiveDaysLater = now.AddDays(5);

            var allCourses = await _context.Courses
                .Include(c => c.Students)
                .Include(c => c.Teacher)
                .ToListAsync();

            var allAssignments = await _context.Assignments
                .Include(a => a.Course)
                .ToListAsync();

            var totalSubmissions = await _context.Submissions.CountAsync(s => !s.IsDeleted);
            var gradedSubmissions = await _context.Submissions.CountAsync(s => !s.IsDeleted && s.IsGraded);
            var pendingSubmissions = totalSubmissions - gradedSubmissions;

            var courseSubmissionStats = await _context.Courses
                .Select(c => new CourseSubmissionCountDto
                {
                    CourseName = c.Name,
                    SubmissionCount = c.Assignments.SelectMany(a => a.Submissions).Count(s => !s.IsDeleted)
                })
                .Where(x => x.SubmissionCount > 0)
                .OrderByDescending(x => x.SubmissionCount)
                .Take(5)
                .ToListAsync();

            var dto = new DashboardDto
            {
                TotalStudents = students.Count,
                TotalTeachers = teachers.Count,
                TotalCourses = allCourses.Count,
                TotalAssignments = allAssignments.Count,

                GradedSubmissions = gradedSubmissions,
                PendingSubmissions = pendingSubmissions,

                CourseSubmissionStats = courseSubmissionStats,

                ActiveAssignments = allAssignments
                    .Where(a => a.DueDate >= now && a.DueDate <= fiveDaysLater)
                    .OrderBy(a => a.DueDate).Take(3)
                    .Select(a => new AssignmentSummaryDto { Id = a.Id, Title = a.Title, CourseName = a.Course?.Name ?? "", DueDate = a.DueDate })
                    .ToList(),

                ExpiredAssignments = allAssignments
                    .Where(a => a.DueDate < now)
                    .OrderByDescending(a => a.DueDate).Take(3)
                    .Select(a => new AssignmentSummaryDto { Id = a.Id, Title = a.Title, CourseName = a.Course?.Name ?? "", DueDate = a.DueDate })
                    .ToList(),

                RecentCourses = allCourses
                    .OrderByDescending(c => c.Id).Take(5)
                    .Select(c => new CourseSummaryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        EnrolledStudentCount = c.Students.Count,
                        TeacherFullName = c.Teacher != null ? c.Teacher.FullName : "Atanmadı"
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        [HttpGet("teacher")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetTeacherDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var now = DateTime.UtcNow;
            var fiveDaysLater = now.AddDays(5);

            var myCourses = await _context.Courses
                .Include(c => c.Students)
                .Where(c => c.TeacherId == userId)
                .ToListAsync();

            var uniqueStudentCount = myCourses.SelectMany(c => c.Students).Select(s => s.Id).Distinct().Count();

            var myAssignments = await _context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.TeacherId == userId)
                .ToListAsync();

            var mySubmissions = await _context.Submissions
                .Include(s => s.Assignment).ThenInclude(a => a.Course)
                .Where(s => s.Assignment.Course.TeacherId == userId && !s.IsDeleted)
                .ToListAsync();

            var gradedSubmissions = mySubmissions.Count(s => s.IsGraded);
            var pendingSubmissions = mySubmissions.Count - gradedSubmissions;

            var courseSubmissionStats = await _context.Courses
                .Where(c => c.TeacherId == userId)
                .Select(c => new CourseSubmissionCountDto
                {
                    CourseName = c.Name,
                    SubmissionCount = c.Assignments.SelectMany(a => a.Submissions).Count(s => !s.IsDeleted)
                })
                .Where(x => x.SubmissionCount > 0)
                .OrderByDescending(x => x.SubmissionCount)
                .Take(5)
                .ToListAsync();

            var dto = new TeacherDashboardDto
            {
                MyTotalStudents = uniqueStudentCount,
                MyTotalCourses = myCourses.Count,
                MyTotalAssignments = myAssignments.Count,

                GradedSubmissions = gradedSubmissions,
                PendingSubmissions = pendingSubmissions,

                CourseSubmissionStats = courseSubmissionStats,

                ActiveAssignments = myAssignments
                    .Where(a => a.DueDate >= now && a.DueDate <= fiveDaysLater)
                    .OrderBy(a => a.DueDate).Take(3)
                    .Select(a => new AssignmentSummaryDto { Id = a.Id, Title = a.Title, CourseName = a.Course.Name, DueDate = a.DueDate })
                    .ToList(),

                ExpiredAssignments = myAssignments
                    .Where(a => a.DueDate < now)
                    .OrderByDescending(a => a.DueDate).Take(3)
                    .Select(a => new AssignmentSummaryDto { Id = a.Id, Title = a.Title, CourseName = a.Course.Name, DueDate = a.DueDate })
                    .ToList(),

                RecentCourses = myCourses
                    .OrderByDescending(c => c.Id).Take(5)
                    .Select(c => new CourseSummaryDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Description = c.Description,
                        EnrolledStudentCount = c.Students.Count,
                        TeacherFullName = ""
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentDashboard()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var now = DateTime.UtcNow;

            var myCourses = await _context.Courses
                .Include(c => c.Teacher)
                .Where(c => c.Students.Any(s => s.Id == userId))
                .ToListAsync();

            var courseIds = myCourses.Select(c => c.Id).ToList();

            var pendingQuery = _context.Assignments
                .Include(a => a.Course)
                .Where(a => courseIds.Contains(a.CourseId) &&
                            a.DueDate >= now &&
                            !a.IsDeleted &&
                            !a.Submissions.Any(s => s.StudentId == userId && !s.IsDeleted));

            var pendingAssignmentsCount = await pendingQuery.CountAsync();

            var upcomingAssignments = await pendingQuery
                .OrderBy(a => a.DueDate)
                .Take(5)
                .ToListAsync();

            var dto = new StudentDashboardDto
            {
                MyEnrolledCoursesCount = myCourses.Count,
                PendingAssignmentsCount = pendingAssignmentsCount,
                MyCourses = myCourses.Select(c => new CourseSummaryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    EnrolledStudentCount = c.Students?.Count ?? 0,
                    TeacherFullName = c.Teacher != null ? c.Teacher.FullName : "Atanmadı"
                }).ToList(),
                UpcomingAssignments = upcomingAssignments.Select(a => new AssignmentSummaryDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CourseName = a.Course?.Name ?? "",
                    DueDate = a.DueDate
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet("student/progress")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetStudentProgress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var progressData = await _context.UserCourseProgresses
                .Include(p => p.Course)
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    CourseName = p.Course.Name,
                    TotalAssignments = p.TotalAssignments,
                    SubmittedAssignments = p.SubmittedAssignments,
                    GradedAssignments = p.GradedAssignments,
                    CompletionRate = p.CompletionRate
                })
                .ToListAsync();

            return Ok(progressData);
        }
    }
}