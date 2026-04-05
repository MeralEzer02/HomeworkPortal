namespace HomeworkPortal.API.DTOs
{
    public class CourseReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeacherId { get; set; }
        public string TeacherFullName { get; set; }
        public DateTime Created { get; set; }
    }
}
