namespace HomeworkPortal.API.DTOs
{
    public class AssignmentReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public DateTime Created {  get; set; }
    }
}
