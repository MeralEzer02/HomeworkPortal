namespace HomeworkPortal.API.DTOs
{
    public class ActionLogDto
    {
        public int Id { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? EntityName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}