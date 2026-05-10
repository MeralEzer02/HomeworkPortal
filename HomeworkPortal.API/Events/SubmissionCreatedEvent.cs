using MediatR;

namespace HomeworkPortal.API.Events
{
    public class SubmissionCreatedEvent : INotification
    {
        public int SubmissionId { get; set; }
        public string StudentId { get; set; }
        public string AssignmentTitle { get; set; }
        public int CourseId { get; set; }
    }
}