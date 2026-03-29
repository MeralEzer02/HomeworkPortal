using Microsoft.AspNetCore.Http;

namespace ÖdevDağıtım.API.DTOs
{
    public class SubmissionCreateDto
    {
        public int AssignmentId { get; set; }
        public IFormFile File { get; set; }
    }
}