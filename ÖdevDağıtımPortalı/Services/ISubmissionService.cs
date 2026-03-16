using ÖdevDağıtım.API.DTOs;

namespace ÖdevDağıtım.API.Services
{
    public interface ISubmissionService
    {
        Task<SubmissionReadDto> SubmitAssignmentAsync(SubmissionCreateDto dto);
        Task GradeSubmissionAsync(SubmissionGradeDto dto);
        Task<IEnumerable<SubmissionReadDto>> GetByAssignmentAsync(int assignmentId);
        Task<IEnumerable<SubmissionReadDto>> GetByStudentAsync(string studentId);
    }
}