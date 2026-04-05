namespace HomeworkPortal.API.DTOs
{
    public class SubmissionReadDto
    {
        public int Id { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string? FilePath { get; set; }
        public bool IsGraded { get; set; }
        public double? Grade { get; set; }
        public int AssignmentId { get; set; }
        public string AssignmentTitle { get; set; }
        public string StudentId { get; set; }
        public string StudentFullName { get; set; }
    }
}
