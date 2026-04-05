namespace HomeworkPortal.API.Models
{
    public class Notification : BaseEntity
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
