using HomeworkPortal.API.DTOs;
using HomeworkPortal.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HomeworkPortal.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _assignmentService;
        private readonly ICurrentUserService _currentUserService;

        public AssignmentsController(IAssignmentService assignmentService, ICurrentUserService currentUserService)
        {
            _assignmentService = assignmentService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateAssignment([FromBody] AssignmentCreateDto dto)
        {
            var result = await _assignmentService.CreateAssignmentAsync(dto);
            return Ok(result);
        }

        [HttpGet("course/{courseId}")]
        public async Task<IActionResult> GetAssignmentsByCourse(int courseId, [FromQuery] PaginationParams paginationParams)
        {
            var result = await _assignmentService.GetAssignmentsByCourseAsync(courseId, paginationParams);
            return Ok(result);
        }

        [HttpGet("student")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAssignmentsForStudent([FromQuery] PaginationParams paginationParams)
        {
            var studentId = _currentUserService.UserId;
            var result = await _assignmentService.GetAssignmentsForStudentAsync(studentId!, paginationParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssignmentDetails(int id)
        {
            var result = await _assignmentService.GetAssignmentDetailsAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateAssignment(int id, [FromBody] AssignmentUpdateDto dto)
        {
            await _assignmentService.UpdateAssignmentAsync(id, dto);
            return Ok(new { message = "Ödev başarıyla güncellendi." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            await _assignmentService.DeleteAssignmentAsync(id);
            return Ok(new { message = "Ödev başarıyla silindi." });
        }

        [HttpGet("for-upload")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetAssignmentsForUpload([FromServices] HomeworkPortal.API.Data.AppDbContext context)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);

            var assignments = await context.Assignments
                .Include(a => a.Course)
                .Where(a => a.Course.Students.Any(s => s.Id == userId) && !a.IsDeleted)
                .Select(a => new {
                    id = a.Id,
                    title = a.Title,
                    courseName = a.Course.Name
                })
                .ToListAsync();

            return Ok(assignments);
        }
    }
}