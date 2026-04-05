using Microsoft.AspNetCore.Identity;

namespace HomeworkPortal.API.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Course> Courses { get; set; }
        public ICollection<Submission> Submissions { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
