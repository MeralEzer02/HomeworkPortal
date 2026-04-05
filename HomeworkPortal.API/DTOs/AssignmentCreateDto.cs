namespace HomeworkPortal.API.DTOs
{
    public class AssignmentCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
    }
}
