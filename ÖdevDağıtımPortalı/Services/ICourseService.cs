using ÖdevDağıtım.API.DTOs;

namespace ÖdevDağıtım.API.Services
{
    public interface ICourseService
    {
        Task<CourseReadDto> CreateCourseAsync(CourseCreateDto dto);
        Task AssignTeacherAsync(int courseId, string teacherId);
        Task EnrollStudentAsync(int courseId, string studentId);
    }
}