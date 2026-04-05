namespace HomeworkPortal.API.DTOs
{
    public class AssignmentUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
