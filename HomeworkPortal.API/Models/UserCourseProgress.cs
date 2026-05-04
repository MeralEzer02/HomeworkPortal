namespace HomeworkPortal.API.Models
{
    public class UserCourseProgress
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public AppUser User { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;


        public int TotalAssignments { get; set; }

        public int SubmittedAssignments { get; set; } 

        public int GradedAssignments { get; set; }

        public int MissedAssignments { get; set; }

        public int PendingAssignments => TotalAssignments - (SubmittedAssignments + GradedAssignments + MissedAssignments);

        public double CompletionRate => TotalAssignments == 0
            ? 0
            : Math.Round((double)(SubmittedAssignments + GradedAssignments) / TotalAssignments * 100, 2);

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}