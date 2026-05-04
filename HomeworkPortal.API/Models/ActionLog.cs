namespace HomeworkPortal.API.Models
{
    public class ActionLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        public AppUser? User { get; set; }

        public string ActionType { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string? EntityName { get; set; }

        public int? EntityId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}