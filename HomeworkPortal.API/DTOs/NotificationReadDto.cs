namespace HomeworkPortal.API.DTOs
{
    public class NotificationReadDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime Created { get; set; }
    }
}
