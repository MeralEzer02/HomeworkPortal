using MediatR;

namespace HomeworkPortal.API.Events
{
    public class SubmissionGradedEvent : INotification
    {
        public int SubmissionId { get; set; }
        public string StudentId { get; set; }
        public string CourseName { get; set; }
        public string AssignmentTitle { get; set; }
        public double Grade { get; set; }
        public string TeacherId { get; set; }
        public int CourseId { get; set; }
    }
}